using GenAccERP.DataAccess.Data;
using GenAccERP.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace GenAccERP.Controllers
{
    internal static class UserResolver
    {
        // Returns the requested user, else the first user, else creates a default admin.
        public static async Task<AppUser> ResolveAsync(AppDbContext db, Guid? userId)
        {
            AppUser? user = null;
            if (userId != null && userId != Guid.Empty)
                user = await db.AppUsers.FindAsync(userId.Value);
            user ??= await db.AppUsers.OrderBy(u => u.CreatedDate).FirstOrDefaultAsync();
            if (user == null)
            {
                user = new AppUser { Username = "admin", Email = "admin@example.com", Role = "Administrator", CreatedDate = DateTime.UtcNow };
                db.AppUsers.Add(user);
                await db.SaveChangesAsync();
            }
            return user;
        }
    }
}
