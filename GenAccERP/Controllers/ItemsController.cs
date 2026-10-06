using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using GenAccERP.Dto;

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

        private static ItemDto ToDto(ItemMaster i) => new ItemDto(i.ItemId, i.CategoryId, i.ItemNo, i.ItemName, i.ItemDescription, i.PurchasePrice, i.SalesPrice, i.CurrentStock, i.Category?.CategoryName, i.CreatedByNavigation != null ? new UserDto(i.CreatedByNavigation.UserId, i.CreatedByNavigation.Username, i.CreatedByNavigation.Email, i.CreatedByNavigation.Role) : null);

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll()
        {
            var items = await _db.ItemMasters.AsNoTracking()
                .Include(i => i.Category)
                .Include(i => i.CreatedByNavigation)
                .OrderBy(i => i.ItemNo)
                .ToListAsync();
            return Ok(items.Select(ToDto));
        }

        // GET: api/items/{id}/stock
        [HttpGet("{id}/stock")]
        public async Task<ActionResult<StockDto>> GetStock(Guid id)
        {
            if (!await _db.ItemMasters.AnyAsync(i => i.ItemId == id))
                return NotFound();
            var purchased = await _db.PurchaseDetails.Where(d => d.ItemId == id).SumAsync(d => (int?)d.Quantity) ?? 0;
            var sold = await _db.SalesDetails.Where(d => d.ItemId == id).SumAsync(d => (int?)d.Quantity) ?? 0;
            return Ok(new StockDto(id, purchased, sold, purchased - sold));
        }

        // PUT: api/items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> Update(Guid id, [FromBody] UpdateItemRequest dto)
        {
            var item = await _db.ItemMasters.FindAsync(id);
            if (item == null)
                return NotFound();
            if (dto.CategoryId != null && dto.CategoryId != Guid.Empty)
            {
                if (!await _db.ItemCategories.AnyAsync(c => c.CategoryId == dto.CategoryId))
                    return BadRequest("Category not found. Provide a valid CategoryId.");
                item.CategoryId = dto.CategoryId.Value;
            }
            if (dto.ItemNo != null) item.ItemNo = dto.ItemNo;
            if (dto.ItemName != null) item.ItemName = dto.ItemName;
            if (dto.ItemDescription != null) item.ItemDescription = dto.ItemDescription;
            if (dto.PurchasePrice != null) item.PurchasePrice = dto.PurchasePrice.Value;
            if (dto.SalesPrice != null) item.SalesPrice = dto.SalesPrice.Value;
            item.ModifiedBy = dto.ModifiedBy;
            item.ModifiedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return await Get(id);
        }

        // DELETE: api/items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _db.ItemMasters.FindAsync(id);
            if (item == null)
                return NotFound();
            _db.ItemMasters.Remove(item);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return Conflict("Item is used in purchases or sales and cannot be deleted."); }
            return NoContent();
        }

        // POST: api/items
        // Insert a new ItemMaster record. If CreatedBy or CategoryId are missing/invalid, the method will create a default admin user or return BadRequest.
        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create([FromBody] CreateItemRequest dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            // Ensure category exists
            var category = await _db.ItemCategories.FirstOrDefaultAsync(c => c.CategoryId == dto.CategoryId);
            if (category == null)
                return BadRequest("Category not found. Provide a valid CategoryId.");

            // Ensure there is at least one AppUser to use as CreatedBy
            AppUser createdByUser;
            if (dto.CreatedBy != null && dto.CreatedBy != Guid.Empty)
            {
                createdByUser = await _db.AppUsers.FindAsync(dto.CreatedBy.Value);
            }
            else
            {
                createdByUser = await _db.AppUsers.FirstOrDefaultAsync();
            }

            if (createdByUser == null)
            {
                createdByUser = new AppUser
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    Role = "Administrator",
                    CreatedDate = DateTime.UtcNow
                };
                _db.AppUsers.Add(createdByUser);
                await _db.SaveChangesAsync();
            }

            var item = new ItemMaster
            {
                ItemId = Guid.NewGuid(),
                ItemNo = dto.ItemNo ?? Guid.NewGuid().ToString(),
                ItemName = dto.ItemName ?? string.Empty,
                ItemDescription = dto.ItemDescription,
                PurchasePrice = dto.PurchasePrice,
                SalesPrice = dto.SalesPrice,
                CurrentStock = dto.CurrentStock,
                CategoryId = category.CategoryId,
                CreatedBy = createdByUser.UserId,
                CreatedDate = DateTime.UtcNow
            };

            _db.ItemMasters.Add(item);
            await _db.SaveChangesAsync();

            var created = new ItemDto(item.ItemId, item.CategoryId, item.ItemNo, item.ItemName, item.ItemDescription, item.PurchasePrice, item.SalesPrice, item.CurrentStock, category.CategoryName, new UserDto(createdByUser.UserId, createdByUser.Username, createdByUser.Email, createdByUser.Role));

            return CreatedAtAction(nameof(Get), new { id = item.ItemId }, created);
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> Get(Guid id)
        {
            var item = await _db.ItemMasters
                .Include(i => i.Category)
                .Include(i => i.CreatedByNavigation)
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
                return NotFound();

            var dto = new ItemDto(item.ItemId, item.CategoryId, item.ItemNo, item.ItemName, item.ItemDescription, item.PurchasePrice, item.SalesPrice, item.CurrentStock, item.Category?.CategoryName, item.CreatedByNavigation != null ? new UserDto(item.CreatedByNavigation.UserId, item.CreatedByNavigation.Username, item.CreatedByNavigation.Email, item.CreatedByNavigation.Role) : null);

            return Ok(dto);
        }
    }

    public class UpdateItemRequest
    {
        public Guid? CategoryId { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string? ItemNo { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalesPrice { get; set; }
    }

    public class CreateItemRequest
    {
        public Guid? CategoryId { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? ItemNo { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesPrice { get; set; }
        public int CurrentStock { get; set; }
    }
}
