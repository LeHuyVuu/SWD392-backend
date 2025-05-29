using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<List<user>> GetAllUserAsync();
        Task<user?> GetUserByIdAsync(int id);
        
        Task AddAsync(user entity);


    }
}
