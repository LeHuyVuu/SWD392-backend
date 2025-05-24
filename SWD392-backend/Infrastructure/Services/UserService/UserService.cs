using SWD392_backend.Infrastructure.Repositories.UserRepository;
using SWD392_backend.Models;

namespace SWD392_backend.Infrastructure.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _userRepository.GetAllUserAsync();
        }
    }
}
