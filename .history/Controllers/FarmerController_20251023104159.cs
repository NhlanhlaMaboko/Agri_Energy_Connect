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
        var users = await _userManager.Users.ToListAsync();
        ViewBag.Users = users;

        if (!ModelState.IsValid)
        {
            Console.WriteLine("Model state invalid:");
            foreach (var kv in ModelState)
            {
                foreach (var error in kv.Value.Errors)
                {
                    Console.WriteLine($"Property: {kv.Key}, Error: {error.ErrorMessage}");
                }
            }
            return View(model);
        }

        // Check if a farmer with the same ApplicationUserId already exists
        var existingFarmer = await _context.Farmers
            .FirstOrDefaultAsync(f => f.ApplicationUserId == model.ApplicationUserId);

        if (existingFarmer != null)
        {
            Console.WriteLine($"Updating existing farmer: {existingFarmer.FarmerId}");
            existingFarmer.FullName = model.FullName;
            existingFarmer.ContactNumber = model.ContactNumber;
            existingFarmer.Location = model.Location;
            existingFarmer.RegistrationSource = model.RegistrationSource;

            _context.Farmers.Update(existingFarmer);
        }
        else
        {
            Console.WriteLine("Adding new farmer...");
            _context.Farmers.Add(model);
        }

        await _context.SaveChangesAsync();
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
