using GenAccERP.DataAccess.Models;

namespace GenAccERP.Services.Interfaces
{
    public interface IReportService
    {
        Task<ServiceResult<IEnumerable<VwItemProfitAndLoss>>> GetItemProfitAndLossAsync();
    }
}
