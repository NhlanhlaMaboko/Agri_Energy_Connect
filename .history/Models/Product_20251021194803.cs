using System;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergy_Connect.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; } 

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Subcategory { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public DateTime ProductionDate { get; set; }

        [Required]
        public int FarmerId { get; set; }

        [Required]
        public Farmer Farmer { get; set; } = new Farmer();
    }
}
