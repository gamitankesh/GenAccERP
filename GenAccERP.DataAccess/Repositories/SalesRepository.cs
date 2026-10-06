using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.DataAccess.Repositories
{
    public class SalesRepository : RepositoryBase, ISalesRepository
    {
        public SalesRepository(AppDbContext db) : base(db) { }

        public Task<List<SalesMaster>> GetAllAsync() =>
            Db.SalesMasters.AsNoTracking().Include(p => p.SalesDetails).OrderBy(p => p.SaleDate).ToListAsync();

        public Task<SalesMaster?> GetByIdAsync(Guid id) =>
            Db.SalesMasters.AsNoTracking().Include(p => p.SalesDetails).FirstOrDefaultAsync(p => p.SaleId == id);

        public Task<SalesMaster?> GetForUpdateAsync(Guid id) =>
            Db.SalesMasters.Include(p => p.SalesDetails).FirstOrDefaultAsync(p => p.SaleId == id);

        public async Task AddAsync(SalesMaster sale) => await Db.SalesMasters.AddAsync(sale);

        public void Remove(SalesMaster sale)
        {
            Db.SalesDetails.RemoveRange(sale.SalesDetails);
            Db.SalesMasters.Remove(sale);
        }
    }
}
