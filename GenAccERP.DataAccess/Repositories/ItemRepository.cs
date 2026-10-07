using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.DataAccess.Repositories
{
    public class ItemRepository : RepositoryBase, IItemRepository
    {
        public ItemRepository(AppDbContext db) : base(db) { }

        public Task<List<ItemMaster>> GetAllWithDetailsAsync() =>
            Db.ItemMasters.AsNoTracking()
                .Include(i => i.Category)
                .Include(i => i.CreatedByNavigation)
                .OrderBy(i => i.ItemNo)
                .ToListAsync();

        public Task<ItemMaster?> GetWithDetailsAsync(Guid id) =>
            Db.ItemMasters
                .Include(i => i.Category)
                .Include(i => i.CreatedByNavigation)
                .FirstOrDefaultAsync(i => i.ItemId == id);

        public async Task<ItemMaster?> GetByIdAsync(Guid id) => await Db.ItemMasters.FindAsync(id);

        public Task<bool> ExistsAsync(Guid id) => Db.ItemMasters.AnyAsync(i => i.ItemId == id);

        public Task<ItemCategory?> GetCategoryAsync(Guid categoryId) =>
            Db.ItemCategories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);

        public async Task<int> GetPurchasedQuantityAsync(Guid itemId) =>
            await Db.PurchaseDetails.Where(d => d.ItemId == itemId).SumAsync(d => (int?)d.Quantity) ?? 0;

        public async Task<int> GetSoldQuantityAsync(Guid itemId) =>
            await Db.SalesDetails.Where(d => d.ItemId == itemId).SumAsync(d => (int?)d.Quantity) ?? 0;

        public async Task AddAsync(ItemMaster item) => await Db.ItemMasters.AddAsync(item);

        public void Remove(ItemMaster item) => Db.ItemMasters.Remove(item);
    }
}
