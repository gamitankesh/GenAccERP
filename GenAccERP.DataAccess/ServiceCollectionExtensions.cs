using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Repositories;
using GenAccERP.DataAccess.Repositories.Interfaces;

namespace GenAccERP.DataAccess
{
    public static class ServiceCollectionExtensions
    {
        private const string DefaultConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GenAccERP;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=\"SQL Server Management Studio\";Command Timeout=0";

        /// <summary>
        /// Registers AppDbContext using the connection string named "DefaultConnection" from configuration.
        /// If not found, falls back to the provided default connection string (localdb, Initial Catalog=GenAccERP).
        /// </summary>
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? DefaultConnectionString;
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            AddRepositories(services);
            return services;
        }

        /// <summary>
        /// Registers AppDbContext with an explicit connection string.
        /// </summary>
        public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            AddRepositories(services);
            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
        }
    }
}
