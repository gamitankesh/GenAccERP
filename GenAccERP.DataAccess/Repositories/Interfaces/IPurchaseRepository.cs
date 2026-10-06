using GenAccERP.DataAccess.Models;

namespace GenAccERP.DataAccess.Repositories.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<List<PurchaseMaster>> GetAllAsync();
        Task<PurchaseMaster?> GetByIdAsync(Guid id);
        /// <summary>Loads a purchase with its lines as a tracked entity for modification.</summary>
        Task<PurchaseMaster?> GetForUpdateAsync(Guid id);
        Task AddAsync(PurchaseMaster purchase);
        void Remove(PurchaseMaster purchase);
        Task SaveChangesAsync();
    }
}
