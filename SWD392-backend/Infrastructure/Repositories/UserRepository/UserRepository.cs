using Microsoft.EntityFrameworkCore;
using SWD392_backend.Models;

namespace SWD392_backend.Infrastructure.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DefaultdbContext _context;

        public UserRepository(DefaultdbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
