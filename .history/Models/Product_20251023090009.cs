public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Category { get; set; }

    public string Subcategory { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public DateTime ProductionDate { get; set; }
}
