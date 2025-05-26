using System.Threading.Tasks;

namespace SWD392_backend.Infrastructure.Services.AuthService
{
    public interface IAuthService
    {
        Task<(bool Success, string Message, string? Token)> LoginAsync(string emailOrPhone, string password);
        Task<(bool Success, string Message)> RegisterAsync(string username, string password, string email);
    }
}