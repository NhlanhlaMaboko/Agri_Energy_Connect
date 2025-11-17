using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgriEnergy_Connect.Data;
using AgriEnergy_Connect.Models;
using Microsoft.AspNetCore.Identity;

[Authorize(Roles = "Employee")]
public class FarmerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public FarmerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Farmer/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var users = await _userManager.Users.ToListAsync();
        ViewBag.Users = users; // Populate dropdown for selecting a user
        return View();
    }

    // POST: Farmer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Farmer model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Users = await _userManager.Users.ToListAsync();
            return View(model);
        }

        try
        {
            // Check if a farmer already exists for this user
            var existingFarmer = await _context.Farmers
                .FirstOrDefaultAsync(f => f.ApplicationUserId == model.ApplicationUserId);

            if (existingFarmer != null)
            {
                // Update existing farmer
                existingFarmer.FullName = model.FullName;
                existingFarmer.ContactNumber = model.ContactNumber;
                existingFarmer.Location = model.Location;

                _context.Farmers.Update(existingFarmer);
            }
            else
            {
                // Add new farmer
                _context.Farmers.Add(model);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Farmer saved successfully!";
            return RedirectToAction("List");
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Failed to save farmer: " + ex.Message;
            ViewBag.Users = await _userManager.Users.ToListAsync();
            return View(model);
        }
    }

    // GET: Farmer/List
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var farmers = await _context.Farmers
            .Include(f => f.ApplicationUser)
            .ToListAsync();

        return View(farmers);
    }
}
