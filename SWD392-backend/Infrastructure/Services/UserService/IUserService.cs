using SWD392_backend.Models;
using SWD392_backend.Infrastructure.Repositories.UserRepository;

namespace SWD392_backend.Infrastructure.Services.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUserAsync();
    }
}
