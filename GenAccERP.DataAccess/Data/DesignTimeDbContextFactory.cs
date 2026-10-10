using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GenAccERP.DataAccess.Data
{
    // Used by EF tooling (dotnet ef) to create a DbContext for design-time operations (migrations)
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // Ensure Initial Catalog is set to GenAccERP
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=GenAccERP;Trusted_Connection=True;MultipleActiveResultSets=True;";
            optionsBuilder.UseSqlServer(connectionString);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
