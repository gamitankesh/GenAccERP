using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ItemsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var items = await _db.ItemMasters
                .Include(i => i.Category)
                .Select(i => new
                {
                    i.ItemId,
                    i.ItemNo,
                    i.ItemName,
                    i.ItemDescription,
                    i.PurchasePrice,
                    i.SalesPrice,
                    i.CurrentStock,
                    Category = i.Category != null ? i.Category.CategoryName : null
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemMaster>> Get(Guid id)
        {
            var item = await _db.ItemMasters
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }
    }
}
