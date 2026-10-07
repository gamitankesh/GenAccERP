using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.DataAccess.Repositories
{
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        public CategoryRepository(AppDbContext db) : base(db) { }

        public Task<List<ItemCategory>> GetAllAsync() => Db.ItemCategories.AsNoTracking().ToListAsync();

        public Task<ItemCategory?> GetWithCreatorAsync(Guid id) =>
            Db.ItemCategories.Include(c => c.CreatedByNavigation).FirstOrDefaultAsync(c => c.CategoryId == id);

        public async Task<ItemCategory?> GetByIdAsync(Guid id) => await Db.ItemCategories.FindAsync(id);

        public async Task AddAsync(ItemCategory category) => await Db.ItemCategories.AddAsync(category);

        public void Remove(ItemCategory category) => Db.ItemCategories.Remove(category);
    }
}
