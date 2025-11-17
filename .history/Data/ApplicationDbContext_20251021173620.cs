using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AgriEnergy_Connect.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Farmer> Farmers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Farmer → ApplicationUser (1:1)
        builder.Entity<Farmer>()
            .HasOne(f => f.ApplicationUser)
            .WithOne(u => u.FarmerProfile)
            .HasForeignKey<Farmer>(f => f.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product → Farmer (many:1)
        builder.Entity<Product>()
            .HasOne(p => p.Farmer)
            .WithMany(f => f.Products)
            .HasForeignKey(p => p.FarmerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Fix decimal precision for Price
        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);
    }
}
