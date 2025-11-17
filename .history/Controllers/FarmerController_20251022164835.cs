using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriEnergy_Connect.Data;
using AgriEnergy_Connect.Models;

[Authorize(Roles = "Employee,Farmer")]
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
    public IActionResult Create()
    {
        return View();
    }

    // POST: Farmer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Farmer model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Get logged-in user
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            ModelState.AddModelError("", "User is not logged in");
            return View(model);
        }

        // Ensure user exists
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ModelState.AddModelError("", "User not found");
            return View(model);
        }

        // Prevent duplicate Farmer profile
        var existingFarmer = await _context.Farmers
            .FirstOrDefaultAsync(f => f.ApplicationUserId == userId);
        if (existingFarmer != null)
        {
            ModelState.AddModelError("", "Farmer profile already exists for this user");
            return View(model);
        }

        // Create Farmer linked to logged-in user
        var farmer = new Farmer
        {
            FullName = model.FullName,
            ContactNumber = model.ContactNumber,
            Location = model.Location,
            RegistrationSource = model.RegistrationSource,
            ApplicationUserId = userId
        };

        _context.Farmers.Add(farmer);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
}
