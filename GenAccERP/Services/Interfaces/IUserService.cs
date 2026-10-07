using GenAccERP.DataAccess.Models;
using GenAccERP.Dto;

namespace GenAccERP.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<IEnumerable<UserDto>>> GetAllAsync();
        Task<ServiceResult<UserDto>> GetAsync(Guid id);
        Task<ServiceResult<UserDto>> CreateAsync(UserRequest request);
        Task<ServiceResult<UserDto>> UpdateAsync(Guid id, UserRequest request);
        Task<ServiceResult<bool>> DeleteAsync(Guid id);

        /// <summary>Returns the requested user, else the first user, else creates a default admin.</summary>
        Task<AppUser> ResolveCreatorAsync(Guid? userId);
    }
}
