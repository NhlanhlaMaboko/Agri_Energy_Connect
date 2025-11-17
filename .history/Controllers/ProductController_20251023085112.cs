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

            return View(products); // Views/Product/AllProducts.cshtml
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

            return View(products); // Views/Product/MyProducts.cshtml
        }

        // ✅ GET: /Product/Create
        [Authorize(Roles = "Farmer")]
        public IActionResult Create()
        {
            return View(); // Views/Product/Create.cshtml
        }

        // ✅ POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.ApplicationUserId == userId);

                if (farmer == null)
                {
                    return Unauthorized("Farmer not found for current user.");
                }

                product.FarmerId = farmer.FarmerId;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyProducts));
            }

            return View(product);
        }
    }
}
