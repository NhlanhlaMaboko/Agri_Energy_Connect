using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergy_Connect.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public string? Subcategory { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        [Required]
        public int FarmerId { get; set; }   // EF will use this as the foreign key

        [ForeignKey("FarmerId")]
        public Farmer? Farmer { get; set; } // Make nullable to avoid ModelState validation errors
    }
}
