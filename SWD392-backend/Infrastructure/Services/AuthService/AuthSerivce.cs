using Microsoft.IdentityModel.Tokens;
using SWD392_backend.Context;
using SWD392_backend.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using web_api_base.Helper;  // sửa theo namespace dự án bạn
using Microsoft.Extensions.Configuration;

namespace SWD392_backend.Infrastructure.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(MyDbContext context, IConfiguration config, IUnitOfWork unitOfWork)
        {
            _context = context;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool Success, string Message, string? Token)> LoginAsync(string emailOrPhone, string password)
        {
            var user = _context.users.FirstOrDefault(u => u.phone == emailOrPhone || u.email == emailOrPhone);
            if (user == null || !PasswordHelper.VerifyPassword(password, user.password))
            {
                return (false, "Sai tên đăng nhập hoặc mật khẩu", null);
            }

            var keyString = _config["Jwt:Key"] ?? throw new Exception("JWT Key is missing in configuration.");
            if (keyString.Length < 32)
            {
                throw new Exception("JWT key must be at least 32 characters (256 bits) for HMAC-SHA256.");
            }

            var key = Encoding.UTF8.GetBytes(keyString);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.id.ToString()),
                    new Claim("FullName", user.full_name ?? ""),
                    new Claim("Role", user.role ?? "")
                }),
                Expires = DateTime.UtcNow.AddMonths(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return (true, "Đăng nhập thành công", jwt);
        }

        public async Task<(bool Success, string Message)> RegisterAsync(string username, string password, string email)
        {
            if (_context.users.Any(u => u.username.ToLower() == username.ToLower()))
                return (false, "Tên đăng nhập đã tồn tại");

            if (_context.users.Any(u => u.email.ToLower() == email.ToLower()))
                return (false, "Email đã được sử dụng");

            var hashedPassword = PasswordHelper.HashPassword(password);

            var newUser = new user
            {
                username = username,
                password = hashedPassword,
                email = email,
                address = string.Empty,
                role = "CUSTOMER",
                full_name = "",
                phone = "",
                image_url = "http://default-avatar.com/avatar.png",
                is_active = true,
                created_at = DateTime.UtcNow
            };

            await _unitOfWork.UserRepository.AddAsync(newUser);
            await _unitOfWork.SaveAsync();

            return (true, "Đăng ký thành công");
        }
    }
}
