using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.DataAccess.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(AppDbContext db) : base(db) { }

        public Task<List<AppUser>> GetAllAsync() => Db.AppUsers.AsNoTracking().OrderBy(u => u.CreatedDate).ToListAsync();

        public async Task<AppUser?> GetByIdAsync(Guid id) => await Db.AppUsers.FindAsync(id);

        public Task<AppUser?> GetFirstAsync() => Db.AppUsers.OrderBy(u => u.CreatedDate).FirstOrDefaultAsync();

        public async Task AddAsync(AppUser user) => await Db.AppUsers.AddAsync(user);

        public void Remove(AppUser user) => Db.AppUsers.Remove(user);
    }
}
