using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories.Interfaces;
using GenAccERP.Services.Interfaces;

namespace GenAccERP.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reports;
        public ReportService(IReportRepository reports) { _reports = reports; }

        public async Task<ServiceResult<IEnumerable<VwItemProfitAndLoss>>> GetItemProfitAndLossAsync() =>
            ServiceResult<IEnumerable<VwItemProfitAndLoss>>.Success(await _reports.GetItemProfitAndLossAsync());
    }
}
