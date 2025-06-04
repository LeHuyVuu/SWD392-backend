using SWD392_backend.Entities;

using SWD392_backend.Infrastructure.Repositories.UserRepository;

namespace SWD392_backend.Infrastructure.Services.UserService
{
    public interface IUserService
    {
        Task<List<user>> GetAllUserAsync();
        Task<user?> GetUserByIdAsync(int id);
        Task<int> GetTotalUserAsync();

    }
}
