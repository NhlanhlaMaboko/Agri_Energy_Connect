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

        // -----------------------------
        // Farmer → ApplicationUser (1:1)
        // -----------------------------
        builder.Entity<Farmer>()
            .HasOne(f => f.ApplicationUser)
            .WithOne(u => u.FarmerProfile)
            .HasForeignKey<Farmer>(f => f.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        // Product → Farmer (many:1)
        // -----------------------------
        builder.Entity<Product>()
            .HasOne(p => p.Farmer)
            .WithMany(f => f.Products)
            .HasForeignKey(p => p.FarmerId)
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        // Decimal precision for Price
        // -----------------------------
        // PostgreSQL uses "numeric", EF Core maps decimal to numeric automatically
        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("numeric(18,2)");

        // -----------------------------
        // Optional: enforce string max length
        // -----------------------------
        builder.Entity<Farmer>()
            .Property(f => f.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Entity<Farmer>()
            .Property(f => f.Location)
            .HasMaxLength(100);
    }
}
