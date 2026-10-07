using GenAccERP.DataAccess.Models;

namespace GenAccERP.DataAccess.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<ItemCategory>> GetAllAsync();
        Task<ItemCategory?> GetWithCreatorAsync(Guid id);
        Task<ItemCategory?> GetByIdAsync(Guid id);
        Task AddAsync(ItemCategory category);
        void Remove(ItemCategory category);
        Task SaveChangesAsync();
    }
}
