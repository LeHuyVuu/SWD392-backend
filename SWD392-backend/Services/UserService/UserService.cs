using SWD392_backend.Models;
using SWD392_backend.Repositories.UserRepository;

namespace SWD392_backend.Services.UserService
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
