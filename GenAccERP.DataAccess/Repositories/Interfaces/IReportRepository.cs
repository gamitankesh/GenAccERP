using GenAccERP.DataAccess.Models;

namespace GenAccERP.DataAccess.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<List<VwItemProfitAndLoss>> GetItemProfitAndLossAsync();
    }
}
