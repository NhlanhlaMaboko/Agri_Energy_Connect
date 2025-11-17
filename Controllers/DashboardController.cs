using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriEnergy_Connect.Data;
using AgriEnergy_Connect.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize(Roles = "Farmer")]
    public async Task<IActionResult> FarmerHome()
    {
        var userId = _userManager.GetUserId(User);

        var farmer = await _context.Farmers
            .FirstOrDefaultAsync(f => f.ApplicationUserId == userId);

        if (farmer == null)
            return RedirectToAction("Register", "Account");

        var products = await _context.Products
            .Where(p => p.FarmerId == farmer.FarmerId)
            .OrderByDescending(p => p.ProductionDate)
            .ToListAsync();

        ViewBag.TotalProducts = products.Count;
        ViewBag.LastProductName = products.FirstOrDefault()?.Name ?? "None";
        ViewBag.LastProductDate = products.FirstOrDefault()?.ProductionDate.ToString("dd MMM yyyy") ?? "N/A";

        return View();
    }

    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> EmployeeHome()
    {
        var products = await _context.Products
            .OrderByDescending(p => p.ProductionDate)
            .ToListAsync();

        ViewBag.TotalProducts = products.Count;
        ViewBag.LastProductName = products.FirstOrDefault()?.Name ?? "None";
        ViewBag.LastProductDate = products.FirstOrDefault()?.ProductionDate.ToString("dd MMM yyyy") ?? "N/A";

        return View();
    }
}
