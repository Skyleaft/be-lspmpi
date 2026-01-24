using be_lspmpi.Data;
using be_lspmpi.Dto;
using be_lspmpi.Models;
using Microsoft.EntityFrameworkCore;

namespace be_lspmpi.Repositories
{
    public class UserRepo(IDBContext db) : IUserRepo
    {
        public async Task<User> Get(Guid id)
        {
            return await db.Users.Include(x => x.Role).Include(x => x.UserProfile)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> Get(string username)
        {
            return await db.Users.Include(x => x.UserProfile).AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User> Create(User user)
        {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(User user)
        {
            var existingUser = await db.Users.Include(x => x.UserProfile).FirstOrDefaultAsync(x => x.Id == user.Id);
            if (existingUser == null)
            {
                return null;
            }
            else
            {
                try
                {
                    existingUser.UserProfile.Name = user.UserProfile.Name;
                    existingUser.UserProfile.Email = user.UserProfile.Email;
                    existingUser.UserProfile.Phone = user.UserProfile.Phone;
                    existingUser.UserProfile.Address = user.UserProfile.Address;
                    existingUser.UserProfile.City = user.UserProfile.City;
                    existingUser.UserProfile.ProfilePicture = user.UserProfile.ProfilePicture;
                    existingUser.UpdatedAt = user.UpdatedAt;
                    existingUser.IsActivated = user.IsActivated;
                    await db.SaveChangesAsync();
                    return existingUser;
                }
                catch (DbUpdateException)
                {
                    throw;
                }
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<PaginatedResponse<User>> Find(FindRequest request)
        {
            var query = db.Users.Include(x => x.UserProfile).AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Username.Contains(request.Search) ||
                                       x.UserProfile.Name.Contains(request.Search) ||
                                       x.UserProfile.Email.Contains(request.Search));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PaginatedResponse<User>
            {
                Data = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}