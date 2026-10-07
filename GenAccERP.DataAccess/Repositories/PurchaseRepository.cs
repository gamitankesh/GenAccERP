using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.DataAccess.Repositories
{
    public class PurchaseRepository : RepositoryBase, IPurchaseRepository
    {
        public PurchaseRepository(AppDbContext db) : base(db) { }

        public Task<List<PurchaseMaster>> GetAllAsync() =>
            Db.PurchaseMasters.AsNoTracking().Include(p => p.PurchaseDetails).OrderBy(p => p.PurchaseDate).ToListAsync();

        public Task<PurchaseMaster?> GetByIdAsync(Guid id) =>
            Db.PurchaseMasters.AsNoTracking().Include(p => p.PurchaseDetails).FirstOrDefaultAsync(p => p.PurchaseId == id);

        public Task<PurchaseMaster?> GetForUpdateAsync(Guid id) =>
            Db.PurchaseMasters.Include(p => p.PurchaseDetails).FirstOrDefaultAsync(p => p.PurchaseId == id);

        public async Task AddAsync(PurchaseMaster purchase) => await Db.PurchaseMasters.AddAsync(purchase);

        public void Remove(PurchaseMaster purchase)
        {
            Db.PurchaseDetails.RemoveRange(purchase.PurchaseDetails);
            Db.PurchaseMasters.Remove(purchase);
        }
    }
}
