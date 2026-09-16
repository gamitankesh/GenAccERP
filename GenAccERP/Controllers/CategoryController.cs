using System;
using System.Collections.Generic;
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
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _db.ItemCategories
                .AsNoTracking()
                .Select(c => new CategoryDto(
                    c.CategoryId,
                    c.CategoryName,
                    c.CreatedBy,
                    c.CreatedDate,
                    null))
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get(Guid id)
        {
            var category = await _db.ItemCategories
                .Include(c => c.CreatedByNavigation)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            var dto = new CategoryDto(
                category.CategoryId,
                category.CategoryName,
                category.CreatedBy,
                category.CreatedDate,
                category.CreatedByNavigation != null ? new UserDto(category.CreatedByNavigation.UserId, category.CreatedByNavigation.Username, category.CreatedByNavigation.Email, category.CreatedByNavigation.Role) : null);

            return Ok(dto);
        }

        // POST: api/category
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryRequest dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.CategoryName))
                return BadRequest("CategoryName is required.");

            AppUser createdBy = null;
            if (dto.CreatedBy != null && dto.CreatedBy != Guid.Empty)
            {
                createdBy = await _db.AppUsers.FindAsync(dto.CreatedBy.Value);
            }

            if (createdBy == null)
            {
                // ensure at least one user exists or create a default admin
                createdBy = await _db.AppUsers.FirstOrDefaultAsync();
                if (createdBy == null)
                {
                    createdBy = new AppUser
                    {
                        Username = "admin",
                        Email = "admin@example.com",
                        Role = "Administrator",
                        CreatedDate = DateTime.UtcNow
                    };
                    _db.AppUsers.Add(createdBy);
                    await _db.SaveChangesAsync();
                }
            }

            var category = new ItemCategory
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = dto.CategoryName,
                CreatedBy = createdBy.UserId,
                CreatedDate = DateTime.UtcNow
            };

            _db.ItemCategories.Add(category);
            await _db.SaveChangesAsync();

            var resultDto = new CategoryDto(category.CategoryId, category.CategoryName, category.CreatedBy, category.CreatedDate, new UserDto(createdBy.UserId, createdBy.Username, createdBy.Email, createdBy.Role));

            return CreatedAtAction(nameof(Get), new { id = category.CategoryId }, resultDto);
        }
    }

    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; } = null!;
        public Guid? CreatedBy { get; set; }
    }
}
