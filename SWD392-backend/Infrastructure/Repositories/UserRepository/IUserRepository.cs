using SWD392_backend.Models;

namespace SWD392_backend.Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUserAsync();
    }
}
