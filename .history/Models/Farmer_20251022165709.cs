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

        // Get logged-in user's GUID
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            ModelState.AddModelError("", "User not logged in.");
            return View(model);
        }

        // Ensure user exists in AspNetUsers
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ModelState.AddModelError("", "Logged-in user not found.");
            return View(model);
        }

        // Prevent duplicate Farmer profile
        var existingFarmer = await _context.Farmers
            .FirstOrDefaultAsync(f => f.ApplicationUserId == userId);
        if (existingFarmer != null)
        {
            ModelState.AddModelError("", "Farmer profile already exists for this user.");
            return View(model);
        }

        // Create Farmer linked to the correct ApplicationUserId (GUID)
        var farmer = new Farmer
        {
            FullName = model.FullName,
            ContactNumber = model.ContactNumber,
            Location = model.Location,
            RegistrationSource = "Self-Registration", // default
            ApplicationUserId = userId
        };

        _context.Farmers.Add(farmer);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
}
