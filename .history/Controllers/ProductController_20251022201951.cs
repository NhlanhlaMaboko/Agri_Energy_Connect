using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriEnergy_Connect.Data;
using AgriEnergy_Connect.Models;
using Microsoft.AspNetCore.Authorization;

namespace AgriEnergy_Connect.Controllers
{
    [Authorize(Roles = "Employee")] // optional
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Product/AllProducts
        public IActionResult AllProducts()
        {
            var products = _context.Products
                .Include(p => p.Farmer)
                .ThenInclude(f => f.ApplicationUser)
                .ToList();

            return View(products);
        }
    }
}
