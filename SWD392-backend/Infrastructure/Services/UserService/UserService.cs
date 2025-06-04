using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.UserRepository;


namespace SWD392_backend.Infrastructure.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<user>> GetAllUserAsync()
        {
            return await _userRepository.GetAllUserAsync();
        }
        
        public async Task<user?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
        
        public async Task<int> GetTotalUserAsync()
        {
            return await _unitOfWork.UserRepository.CountAsync();
        }


    }
}
