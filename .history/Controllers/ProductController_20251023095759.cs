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
       
    }
}
