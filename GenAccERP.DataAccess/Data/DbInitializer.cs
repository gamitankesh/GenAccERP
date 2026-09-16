using System.Linq;
using GenAccERP.DataAccess.Entities;

namespace GenAccERP.DataAccess.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Ensure database is created. For real projects prefer migrations.
            context.Database.EnsureCreated();

            if (context.Categories.Any())
            {
                return;   // DB has been seeded
            }

            var categories = new Category[]
            {
                new Category { Name = "General" },
                new Category { Name = "Services" },
                new Category { Name = "Products" }
            };

            foreach (var c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var items = new Item[]
            {
                new Item { Name = "Sample Item A", Description = "Seed item A", Price = 10.0M, CategoryId = categories[0].Id },
                new Item { Name = "Sample Item B", Description = "Seed item B", Price = 20.5M, CategoryId = categories[2].Id }
            };
            foreach (var i in items)
            {
                context.Items.Add(i);
            }
            context.SaveChanges();
        }
    }
}
