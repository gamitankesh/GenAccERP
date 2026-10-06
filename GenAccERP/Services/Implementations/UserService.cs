using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories;
using GenAccERP.DataAccess.Repositories.Interfaces;
using GenAccERP.Dto;
using GenAccERP.Services.Interfaces;

namespace GenAccERP.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _users;
        public UserService(IUserRepository users) { _users = users; }

        private static UserDto ToDto(AppUser u) => new(u.UserId, u.Username, u.Email, u.Role);

        public async Task<ServiceResult<IEnumerable<UserDto>>> GetAllAsync()
        {
            var users = await _users.GetAllAsync();
            return ServiceResult<IEnumerable<UserDto>>.Success(users.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<UserDto>> GetAsync(Guid id)
        {
            var u = await _users.GetByIdAsync(id);
            return u == null ? ServiceResult<UserDto>.NotFound() : ServiceResult<UserDto>.Success(ToDto(u));
        }

        public async Task<ServiceResult<UserDto>> CreateAsync(UserRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email))
                return ServiceResult<UserDto>.BadRequest("Username and Email are required.");

            var creator = await ResolveCreatorAsync(request.CreatedBy);
            var u = new AppUser
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                Role = string.IsNullOrWhiteSpace(request.Role) ? "User" : request.Role,
                CreatedBy = creator.UserId,
                CreatedDate = DateTime.UtcNow
            };
            await _users.AddAsync(u);
            try { await _users.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<UserDto>.BadRequest(ex.Message); }
            return ServiceResult<UserDto>.Success(ToDto(u));
        }

        public async Task<ServiceResult<UserDto>> UpdateAsync(Guid id, UserRequest request)
        {
            var u = await _users.GetByIdAsync(id);
            if (u == null) return ServiceResult<UserDto>.NotFound();
            if (!string.IsNullOrWhiteSpace(request.Username)) u.Username = request.Username;
            if (!string.IsNullOrWhiteSpace(request.Email)) u.Email = request.Email;
            if (!string.IsNullOrWhiteSpace(request.Role)) u.Role = request.Role;
            u.ModifiedDate = DateTime.UtcNow;
            u.ModifiedBy = request.ModifiedBy;
            try { await _users.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<UserDto>.BadRequest(ex.Message); }
            return ServiceResult<UserDto>.Success(ToDto(u));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
        {
            var u = await _users.GetByIdAsync(id);
            if (u == null) return ServiceResult<bool>.NotFound();
            _users.Remove(u);
            try { await _users.SaveChangesAsync(); }
            catch (DataAccessException) { return ServiceResult<bool>.Conflict("User is referenced by other records and cannot be deleted."); }
            return ServiceResult<bool>.Success(true);
        }

        public async Task<AppUser> ResolveCreatorAsync(Guid? userId)
        {
            AppUser? user = null;
            if (userId != null && userId != Guid.Empty)
                user = await _users.GetByIdAsync(userId.Value);
            user ??= await _users.GetFirstAsync();
            if (user == null)
            {
                user = new AppUser { Username = "admin", Email = "admin@example.com", Role = "Administrator", CreatedDate = DateTime.UtcNow };
                await _users.AddAsync(user);
                await _users.SaveChangesAsync();
            }
            return user;
        }
    }
}
