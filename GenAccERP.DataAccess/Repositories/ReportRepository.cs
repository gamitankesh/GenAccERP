using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.DataAccess.Repositories
{
    public class ReportRepository : RepositoryBase, IReportRepository
    {
        public ReportRepository(AppDbContext db) : base(db) { }

        public Task<List<VwItemProfitAndLoss>> GetItemProfitAndLossAsync() =>
            Db.VwItemProfitAndLosses.AsNoTracking().ToListAsync();
    }
}
