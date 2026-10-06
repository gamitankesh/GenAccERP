using GenAccERP.DataAccess.Models;

namespace GenAccERP.DataAccess.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<List<ItemMaster>> GetAllWithDetailsAsync();
        Task<ItemMaster?> GetWithDetailsAsync(Guid id);
        Task<ItemMaster?> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<ItemCategory?> GetCategoryAsync(Guid categoryId);
        Task<int> GetPurchasedQuantityAsync(Guid itemId);
        Task<int> GetSoldQuantityAsync(Guid itemId);
        Task AddAsync(ItemMaster item);
        void Remove(ItemMaster item);
        Task SaveChangesAsync();
    }
}
