using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AgriEnergy_Connect.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Replace this with your actual connection string
            optionsBuilder.UseSqlServer("Server=(localdb)\\v11.0;Database=AgriEnergyConnectDB;Trusted_Connection=True;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
dotnet ef migrations remove --context AgriEnergy_Connect.Data.ApplicationDbContext
