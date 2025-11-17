using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergy_Connect.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Full name for both Farmers and Employees
        [Required]
        public string Name { get; set; } = string.Empty;

        // User type: Farmer or Employee
        [Required]
        public string UserType { get; set; } = string.Empty;

        // Optional: link to Farmer profile (only if UserType == "Farmer")
        public Farmer? FarmerProfile { get; set; }
    }
}
