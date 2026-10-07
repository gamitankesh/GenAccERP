using GenAccERP.DataAccess.Models;

namespace GenAccERP.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(Guid id);
        Task<AppUser?> GetFirstAsync();
        Task AddAsync(AppUser user);
        void Remove(AppUser user);
        Task SaveChangesAsync();
    }
}
