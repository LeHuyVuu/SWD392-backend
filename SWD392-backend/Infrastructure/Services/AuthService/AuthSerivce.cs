
using SWD392_backend.Context;
using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Services.AuthService;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_api_base.Helper; // Đảm bảo namespace chứa PasswordHelper đúng với dự án bạn
public class AuthService
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
        var user = _context.users.FirstOrDefault(u => u.Phone == emailOrPhone || u.Email == emailOrPhone);
        if (user == null || !PasswordHelper.VerifyPassword(password, user.Password))
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
                new Claim("UserId", user.Id.ToString()),
                new Claim("FullName", user.FullName ?? ""),
                new Claim("Role", user.Role ?? "")
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
        if (_context.users.Any(u => u.Username.ToLower() == username.ToLower()))
            return (false, "Tên đăng nhập đã tồn tại");

        if (_context.users.Any(u => u.Email.ToLower() == email.ToLower()))
            return (false, "Email đã được sử dụng");

        var hashedPassword = PasswordHelper.HashPassword(password);

        var newUser = new user
        {
            Username = username,
            Password = hashedPassword,
            Email = email,
            Address = string.Empty,
            Role = "CUSTOMER",
            FullName = "",
            Phone = "",
            ImageUrl = "http://default-avatar.com/avatar.png",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.UserRepository.AddAsync(newUser);
        await _unitOfWork.SaveAsync();
        return (true, "Đăng ký thành công");
    }
}
