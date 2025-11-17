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
    public async Task<IActionResult> Create()
    {
        var users = await _userManager.Users.ToListAsync();
        ViewBag.Users = users;
        return View();
    }

    // POST: Farmer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Farmer model)
    {
        Console.WriteLine("POST Create hit");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("ModelState is invalid!");
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = users;
            return View(model);
        }

        // Check if a farmer already exists for this ApplicationUserId
        var existingFarmer = await _context.Farmers
            .FirstOrDefaultAsync(f => f.ApplicationUserId == model.ApplicationUserId);

        if (existingFarmer != null)
        {
            Console.WriteLine($"Farmer already exists for UserId: {model.ApplicationUserId}, updating existing farmer...");

            // Update existing farmer
            existingFarmer.Name = model.Name;
            existingFarmer.Contact = model.Contact;
            existingFarmer.Location = model.Location;

            _context.Farmers.Update(existingFarmer);
        }
        else
        {
            Console.WriteLine($"Adding new farmer for UserId: {model.ApplicationUserId}");
            _context.Farmers.Add(model);
        }

        try
        {
            await _context.SaveChangesAsync();
            Console.WriteLine("SaveChangesAsync completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving farmer: {ex.Message}");
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = users;
            ModelState.AddModelError("", "An error occurred while saving the farmer. See console for details.");
            return View(model);
        }

        return RedirectToAction("List");
    }

    // GET: Farmer/List
    public async Task<IActionResult> List()
    {
        var farmers = await _context.Farmers
            .Include(f => f.ApplicationUser)
            .ToListAsync();

        return View(farmers);
    }
}
