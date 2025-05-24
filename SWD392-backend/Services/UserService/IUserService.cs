using SWD392_backend.Models;
using SWD392_backend.Repositories.UserRepository;

namespace SWD392_backend.Services.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetAllUserAsync();
    }
}
