using Microsoft.EntityFrameworkCore;
using SWD392_backend.Context;
using SWD392_backend.Entities;

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
    }
}
