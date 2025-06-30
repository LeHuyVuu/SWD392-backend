using Microsoft.EntityFrameworkCore;
using SWD392_backend.Context;
using SWD392_backend.Entities;
using Microsoft.EntityFrameworkCore;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;

        public UserRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<user>> GetAllUserAsync()
        {
            return await _context.users.ToListAsync();
        }
        public async Task<user?> GetUserByIdAsync(int id)
        {
            return await _context.users.FindAsync(id);
        }

        public async Task AddAsync(user entity)
        {
            await _context.users.AddAsync(entity);
        }
        public async Task<int> CountAsync()
        {
          return  await _context.users.CountAsync();
        }

        public async Task<user?> GetUserByEmail(string requestEmail)
        {
            return await _context.users.Where(u => u.Email == requestEmail).FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalUserByMonth(int month, int year)
        {
            var start = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.AddMonths(1);
            return await _context.users
                .Where(u => u.CreatedAt >= start && u.CreatedAt < end)
                .CountAsync();
        }

        public async Task<int> CountUsersBetween(DateTime start, DateTime end)
        {
            return await _context.users
                .Where(u => u.CreatedAt >= start && u.CreatedAt < end)
                .CountAsync();
        }

    }
}
