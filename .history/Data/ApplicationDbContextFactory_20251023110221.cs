using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using AgriEnergy_Connect.Models;

namespace AgriEnergy_Connect.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // PostgreSQL connection string (Supabase)
            var connectionString = "Host=db.yhuwpeueuxcytmyzrust.supabase.co;Database=postgres;Username=postgres;Password=MissNhlahla@123;SSL Mode=Require;Trust Server Certificate=true";

            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

