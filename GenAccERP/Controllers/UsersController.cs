using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using GenAccERP.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) { _db = db; }

        private static UserDto ToDto(AppUser u) => new(u.UserId, u.Username, u.Email, u.Role);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _db.AppUsers.AsNoTracking().OrderBy(u => u.CreatedDate).ToListAsync();
            return Ok(users.Select(ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var u = await _db.AppUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);
            return u == null ? NotFound() : Ok(ToDto(u));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserRequest dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Username and Email are required.");
            var creator = await UserResolver.ResolveAsync(_db, dto.CreatedBy);
            var u = new AppUser
            {
                UserId = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role,
                CreatedBy = creator.UserId,
                CreatedDate = DateTime.UtcNow
            };
            _db.AppUsers.Add(u);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = u.UserId }, ToDto(u));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] UserRequest dto)
        {
            var u = await _db.AppUsers.FindAsync(id);
            if (u == null) return NotFound();
            if (!string.IsNullOrWhiteSpace(dto.Username)) u.Username = dto.Username;
            if (!string.IsNullOrWhiteSpace(dto.Email)) u.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Role)) u.Role = dto.Role;
            u.ModifiedDate = DateTime.UtcNow;
            u.ModifiedBy = dto.ModifiedBy;
            await _db.SaveChangesAsync();
            return Ok(ToDto(u));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var u = await _db.AppUsers.FindAsync(id);
            if (u == null) return NotFound();
            _db.AppUsers.Remove(u);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return Conflict("User is referenced by other records and cannot be deleted."); }
            return NoContent();
        }
    }

    public class UserRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
