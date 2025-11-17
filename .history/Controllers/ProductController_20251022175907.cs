using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriEnergy_Connect.Data;
using AgriEnergy_Connect.Models;

namespace AgriEnergy_Connect.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Product/AllProducts
        public async Task<IActionResult> AllProducts()
        {
            // Include Farmer details if you want to display them in the view
            var products = await _context.Products
                                         .Include(p => p.Farmer)
                                         .ThenInclude(f => f.ApplicationUser) // optional, to show farmer name/email
                                         .ToListAsync();
            return View(products);
        }

        // Optional: GET: /Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                                        .Include(p => p.Farmer)
                                        .ThenInclude(f => f.ApplicationUser)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // Optional: GET: /Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // Optional: POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllProducts));
            }
            return View(product);
        }
    }
}
