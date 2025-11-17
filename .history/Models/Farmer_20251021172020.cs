using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergy_Connect.Models
{
    public class Farmer
    {
        public int FarmerId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        // Optional alias for legacy references to 'Name'
        public string Name
        {
            get => FullName;
            set => FullName = value;
        }

        public string? ContactNumber { get; set; }
        public string? Location { get; set; }
        public string? RegistrationSource { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        // Navigation property — do NOT instantiate here to avoid EF tracking issues
        public ApplicationUser? ApplicationUser { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
