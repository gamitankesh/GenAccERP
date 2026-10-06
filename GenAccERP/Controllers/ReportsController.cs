using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ReportsController(AppDbContext db) { _db = db; }

        // GET: api/reports/item-profit-loss
        [HttpGet("item-profit-loss")]
        public async Task<ActionResult<IEnumerable<VwItemProfitAndLoss>>> ItemProfitAndLoss()
            => Ok(await _db.VwItemProfitAndLosses.AsNoTracking().ToListAsync());
    }
}
