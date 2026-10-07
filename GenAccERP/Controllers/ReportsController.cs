using GenAccERP.DataAccess.Models;
using GenAccERP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        // GET: api/reports/item-profit-loss
        [HttpGet("item-profit-loss")]
        public async Task<ActionResult<IEnumerable<VwItemProfitAndLoss>>> ItemProfitAndLoss() =>
            this.ToActionResult(await _service.GetItemProfitAndLossAsync());
    }
}
