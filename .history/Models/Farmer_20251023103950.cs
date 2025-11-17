using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergy_Connect.Models
{
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [NotMapped] // Optional convenience property
        public string Name
        {
            get => FullName;
            set => FullName = value;
        }

       

        public string? Location { get; set; }

        public string? RegistrationSource { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        public ApplicationUser? ApplicationUser { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
