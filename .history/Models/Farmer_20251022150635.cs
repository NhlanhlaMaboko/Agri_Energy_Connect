using System.ComponentModel.DataAnnotations.Schema;

public class Farmer
{
    public int FarmerId { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [NotMapped]   // <-- ignore this property in EF
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

    public ApplicationUser? ApplicationUser { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
