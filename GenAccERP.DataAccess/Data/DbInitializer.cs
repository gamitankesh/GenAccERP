using System;
using System.Linq;
using GenAccERP.DataAccess.Models;

namespace GenAccERP.DataAccess.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Ensure database is created. For real projects prefer migrations and Database.Migrate().
            context.Database.EnsureCreated();

            // If item categories exist assume DB has been seeded
            if (context.ItemCategories.Any())
            {
                return;
            }

            // Create an initial admin user to satisfy foreign key constraints
            var admin = new AppUser
            {
                Username = "admin",
                Email = "admin@example.com",
                Role = "Administrator",
                CreatedDate = DateTime.UtcNow
            };
            context.AppUsers.Add(admin);
            context.SaveChanges();

            // Create categories
            var categories = new ItemCategory[]
            {
                new ItemCategory { CategoryName = "General", CreatedBy = admin.UserId },
                new ItemCategory { CategoryName = "Services", CreatedBy = admin.UserId },
                new ItemCategory { CategoryName = "Products", CreatedBy = admin.UserId }
            };

            context.ItemCategories.AddRange(categories);
            context.SaveChanges();

            // Create sample item masters
            var items = new ItemMaster[]
            {
                new ItemMaster { ItemNo = "ITM-001", ItemName = "Sample Item A", ItemDescription = "Seed item A", PurchasePrice = 5.00M, SalesPrice = 10.00M, CurrentStock = 100, CategoryId = categories[0].CategoryId, CreatedBy = admin.UserId },
                new ItemMaster { ItemNo = "ITM-002", ItemName = "Sample Item B", ItemDescription = "Seed item B", PurchasePrice = 15.00M, SalesPrice = 25.00M, CurrentStock = 50, CategoryId = categories[2].CategoryId, CreatedBy = admin.UserId }
            };

            context.ItemMasters.AddRange(items);
            context.SaveChanges();
        }
    }
}
