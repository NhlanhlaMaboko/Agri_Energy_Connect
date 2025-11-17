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

{
    if (!ModelState.IsValid)
    {
        return View(product);
    }

    try
    {
        // Get logged-in user ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Get the farmer record
        var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.ApplicationUserId == userId);
        if (farmer == null)
        {
            // Automatically create farmer if not exists
            farmer = new Farmer
            {
                FullName = User.Identity.Name ?? "Default Name",
                ContactNumber = "",
                Location = "",
                RegistrationSource = "Self",
                ApplicationUserId = userId
            };

            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();
        }

        // Use raw SQL to insert
        await _context.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO Products (Name, Category, Subcategory, Price, ProductionDate, FarmerId)
            VALUES ({product.Name}, {product.Category}, {product.Subcategory}, {product.Price}, {product.ProductionDate}, {farmer.FarmerId})
        ");

        return RedirectToAction(nameof(MyProducts));
    }
    catch (Exception ex)
    {
        // Log the exception to console (Output window in Visual Studio)
        Console.WriteLine("Error inserting product: " + ex.Message);
        Console.WriteLine(ex.StackTrace);

        // Optionally, display error on page
        ModelState.AddModelError(string.Empty, "Failed to save product. See console for details.");
        return View(product);
    }
}

    }
}
