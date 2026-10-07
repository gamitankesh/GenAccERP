using GenAccERP.DataAccess.Models;

namespace GenAccERP.DataAccess.Repositories.Interfaces
{
    public interface ISalesRepository
    {
        Task<List<SalesMaster>> GetAllAsync();
        Task<SalesMaster?> GetByIdAsync(Guid id);
        /// <summary>Loads a sale with its lines as a tracked entity for modification.</summary>
        Task<SalesMaster?> GetForUpdateAsync(Guid id);
        Task AddAsync(SalesMaster sale);
        void Remove(SalesMaster sale);
        Task SaveChangesAsync();
    }
}
