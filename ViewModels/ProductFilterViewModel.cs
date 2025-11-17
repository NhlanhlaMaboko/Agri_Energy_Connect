using AgriEnergy_Connect.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgriEnergy_Connect.ViewModels
{
    public class ProductFilterViewModel
    {
        public int? FarmerId { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<Product> Products { get; set; } = new();
        public List<SelectListItem> Farmers { get; set; } = new();
    }
}
