using AgriEnergy_Connect.Data;
using AgriEnergy_Connect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgriEnergy_Connect.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Product/AllProducts
        [AllowAnonymous]
        public async Task<IActionResult> AllProducts()
        {
            var products = await _context.Products
                                         .Include(p => p.Farmer)
                                         .ThenInclude(f => f.ApplicationUser)
                                         .ToListAsync();
            return View(products);
        }

        // GET: /Product/MyProducts
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> MyProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var products = await _context.Products
                                         .Include(p => p.Farmer)
                                         .Where(p => p.Farmer.ApplicationUserId == userId)
                                         .ToListAsync();

            return View(products);
        }

        // GET: /Product/Create
        [Authorize(Roles = "Farmer")]
        public IActionResult Create()
        {
            return View(); // Show the add product form
        }

        // POST: /Product/Create
       // POST: /Product/Create
[HttpPost]
[ValidateAntiForgeryToken]
[Authorize(Roles = "Farmer")]
public async Task<IActionResult> Create(Product product)
{
    Console.WriteLine("POST Create hit"); // Check if action is called

    if (!ModelState.IsValid)
    {
        Console.WriteLine("ModelState is invalid!");
        foreach (var state in ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                Console.WriteLine($"Property: {state.Key}, Error: {error.ErrorMessage}");
            }
        }
        return View(product);
    }

    try
    {
        // Get logged-in user ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            Console.WriteLine("User not logged in or UserId not found");
            return Unauthorized();
        }

        // Get the Farmer record
        var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.ApplicationUserId == userId);

        if (farmer == null)
        {
            // Automatically create farmer if not exists
            farmer = new Farmer
            {
                FullName = User.Identity.Name ?? "Default Name",
                Contact = "",
                Location = "",
                RegistrationSource = "Self",
                ApplicationUserId = userId
            };

            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Created new Farmer: {farmer.FullName}, Id: {farmer.FarmerId}");
        }

        // Assign FarmerId & navigation property
        product.FarmerId = farmer.FarmerId;
        product.Farmer = farmer;

        // Save product to database using EF Core
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        Console.WriteLine($"Product saved: {product.Name}, Id: {product.ProductId}");

        return RedirectToAction(nameof(MyProducts));
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception while saving product: " + ex.Message);
        Console.WriteLine(ex.StackTrace);
        ModelState.AddModelError(string.Empty, "An error occurred while saving the product.");
        return View(product);
    }
}


    }
}
