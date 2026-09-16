using System;
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
