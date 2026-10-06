using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using GenAccERP.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public SalesController(AppDbContext db) { _db = db; }

        private static SaleDto ToDto(SalesMaster p) => new(
            p.SaleId, p.SaleDate, p.CustomerName, p.TotalAmount, p.CreatedDate,
            p.SalesDetails.Select(d => new SaleLineDto(d.SalesDetailId, d.ItemId, d.Quantity, d.UnitPrice, d.LineTotal)).ToList());

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetAll()
        {
            var list = await _db.SalesMasters.AsNoTracking().Include(p => p.SalesDetails).OrderBy(p => p.SaleDate).ToListAsync();
            return Ok(list.Select(ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> Get(Guid id)
        {
            var p = await _db.SalesMasters.AsNoTracking().Include(x => x.SalesDetails).FirstOrDefaultAsync(x => x.SaleId == id);
            return p == null ? NotFound() : Ok(ToDto(p));
        }

        [HttpPost]
        public async Task<ActionResult<SaleDto>> Create([FromBody] SaleRequest dto)
        {
            if (dto == null || dto.ItemId == null || dto.Quantity <= 0)
                return BadRequest("ItemId and a positive Quantity are required.");
            var item = await _db.ItemMasters.FindAsync(dto.ItemId.Value);
            if (item == null) return BadRequest("Item not found.");
            var user = await UserResolver.ResolveAsync(_db, dto.CreatedBy);
            var unitPrice = dto.UnitPrice ?? item.SalesPrice;

            var master = new SalesMaster
            {
                SaleId = Guid.NewGuid(),
                SaleDate = dto.SaleDate ?? DateTime.UtcNow,
                CustomerName = dto.CustomerName,
                TotalAmount = unitPrice * dto.Quantity,
                CreatedBy = user.UserId,
                CreatedDate = DateTime.UtcNow
            };
            master.SalesDetails.Add(new SalesDetail
            {
                SalesDetailId = Guid.NewGuid(),
                SaleId = master.SaleId,
                ItemId = item.ItemId,
                Quantity = dto.Quantity,
                UnitPrice = unitPrice
            });
            _db.SalesMasters.Add(master);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException ex) { return BadRequest(ex.InnerException?.Message ?? ex.Message); }
            var created = await _db.SalesMasters.AsNoTracking().Include(x => x.SalesDetails).FirstAsync(x => x.SaleId == master.SaleId);
            return CreatedAtAction(nameof(Get), new { id = master.SaleId }, ToDto(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SaleDto>> Update(Guid id, [FromBody] SaleRequest dto)
        {
            var master = await _db.SalesMasters.Include(p => p.SalesDetails).FirstOrDefaultAsync(p => p.SaleId == id);
            if (master == null) return NotFound();
            if (dto.SaleDate != null) master.SaleDate = dto.SaleDate.Value;
            if (dto.CustomerName != null) master.CustomerName = dto.CustomerName;

            var line = master.SalesDetails.FirstOrDefault();
            if (line != null && dto.Quantity > 0)
            {
                if (dto.ItemId != null && dto.ItemId != line.ItemId)
                {
                    var item = await _db.ItemMasters.FindAsync(dto.ItemId.Value);
                    if (item == null) return BadRequest("Item not found.");
                    line.ItemId = item.ItemId;
                    line.UnitPrice = dto.UnitPrice ?? item.SalesPrice;
                }
                else if (dto.UnitPrice != null) line.UnitPrice = dto.UnitPrice.Value;
                line.Quantity = dto.Quantity;
            }
            master.TotalAmount = master.SalesDetails.Sum(d => d.Quantity * d.UnitPrice);
            master.ModifiedDate = DateTime.UtcNow;
            master.ModifiedBy = dto.ModifiedBy;
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException ex) { return BadRequest(ex.InnerException?.Message ?? ex.Message); }
            var updated = await _db.SalesMasters.AsNoTracking().Include(x => x.SalesDetails).FirstAsync(x => x.SaleId == id);
            return Ok(ToDto(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var master = await _db.SalesMasters.Include(p => p.SalesDetails).FirstOrDefaultAsync(p => p.SaleId == id);
            if (master == null) return NotFound();
            _db.SalesDetails.RemoveRange(master.SalesDetails);
            _db.SalesMasters.Remove(master);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }

    public class SaleRequest
    {
        public Guid? ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? SaleDate { get; set; }
        public string? CustomerName { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
