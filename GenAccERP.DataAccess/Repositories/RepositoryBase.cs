using GenAccERP.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.DataAccess.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly AppDbContext Db;

        protected RepositoryBase(AppDbContext db) { Db = db; }

        public async Task SaveChangesAsync()
        {
            try { await Db.SaveChangesAsync(); }
            catch (DbUpdateException ex) { throw new DataAccessException(ex.InnerException?.Message ?? ex.Message, ex); }
        }
    }
}
