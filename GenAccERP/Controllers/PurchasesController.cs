using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using GenAccERP.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PurchasesController(AppDbContext db) { _db = db; }

        private static PurchaseDto ToDto(PurchaseMaster p) => new(
            p.PurchaseId, p.PurchaseDate, p.SupplierName, p.TotalAmount, p.CreatedDate,
            p.PurchaseDetails.Select(d => new PurchaseLineDto(d.PurchaseDetailId, d.ItemId, d.Quantity, d.UnitPrice, d.LineTotal)).ToList());

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetAll()
        {
            var list = await _db.PurchaseMasters.AsNoTracking().Include(p => p.PurchaseDetails).OrderBy(p => p.PurchaseDate).ToListAsync();
            return Ok(list.Select(ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseDto>> Get(Guid id)
        {
            var p = await _db.PurchaseMasters.AsNoTracking().Include(x => x.PurchaseDetails).FirstOrDefaultAsync(x => x.PurchaseId == id);
            return p == null ? NotFound() : Ok(ToDto(p));
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseDto>> Create([FromBody] PurchaseRequest dto)
        {
            if (dto == null || dto.ItemId == null || dto.Quantity <= 0)
                return BadRequest("ItemId and a positive Quantity are required.");
            var item = await _db.ItemMasters.FindAsync(dto.ItemId.Value);
            if (item == null) return BadRequest("Item not found.");
            var user = await UserResolver.ResolveAsync(_db, dto.CreatedBy);
            var unitPrice = dto.UnitPrice ?? item.PurchasePrice;

            var master = new PurchaseMaster
            {
                PurchaseId = Guid.NewGuid(),
                PurchaseDate = dto.PurchaseDate ?? DateTime.UtcNow,
                SupplierName = dto.SupplierName,
                TotalAmount = unitPrice * dto.Quantity,
                CreatedBy = user.UserId,
                CreatedDate = DateTime.UtcNow
            };
            master.PurchaseDetails.Add(new PurchaseDetail
            {
                PurchaseDetailId = Guid.NewGuid(),
                PurchaseId = master.PurchaseId,
                ItemId = item.ItemId,
                Quantity = dto.Quantity,
                UnitPrice = unitPrice
            });
            _db.PurchaseMasters.Add(master);
            await _db.SaveChangesAsync();
            var created = await _db.PurchaseMasters.AsNoTracking().Include(x => x.PurchaseDetails).FirstAsync(x => x.PurchaseId == master.PurchaseId);
            return CreatedAtAction(nameof(Get), new { id = master.PurchaseId }, ToDto(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PurchaseDto>> Update(Guid id, [FromBody] PurchaseRequest dto)
        {
            var master = await _db.PurchaseMasters.Include(p => p.PurchaseDetails).FirstOrDefaultAsync(p => p.PurchaseId == id);
            if (master == null) return NotFound();
            if (dto.PurchaseDate != null) master.PurchaseDate = dto.PurchaseDate.Value;
            if (dto.SupplierName != null) master.SupplierName = dto.SupplierName;

            var line = master.PurchaseDetails.FirstOrDefault();
            if (line != null && dto.Quantity > 0)
            {
                if (dto.ItemId != null && dto.ItemId != line.ItemId)
                {
                    var item = await _db.ItemMasters.FindAsync(dto.ItemId.Value);
                    if (item == null) return BadRequest("Item not found.");
                    line.ItemId = item.ItemId;
                    line.UnitPrice = dto.UnitPrice ?? item.PurchasePrice;
                }
                else if (dto.UnitPrice != null) line.UnitPrice = dto.UnitPrice.Value;
                line.Quantity = dto.Quantity;
            }
            master.TotalAmount = master.PurchaseDetails.Sum(d => d.Quantity * d.UnitPrice);
            master.ModifiedDate = DateTime.UtcNow;
            master.ModifiedBy = dto.ModifiedBy;
            await _db.SaveChangesAsync();
            var updated = await _db.PurchaseMasters.AsNoTracking().Include(x => x.PurchaseDetails).FirstAsync(x => x.PurchaseId == id);
            return Ok(ToDto(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var master = await _db.PurchaseMasters.Include(p => p.PurchaseDetails).FirstOrDefaultAsync(p => p.PurchaseId == id);
            if (master == null) return NotFound();
            _db.PurchaseDetails.RemoveRange(master.PurchaseDetails);
            _db.PurchaseMasters.Remove(master);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }

    public class PurchaseRequest
    {
        public Guid? ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? SupplierName { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
