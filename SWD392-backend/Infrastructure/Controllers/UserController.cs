using cybersoft_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SWD392_backend.Infrastructure.Services.UserService;
using System.Text.Json;

namespace SWD392_backend.Infrastructure.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDistributedCache _cache;

        public UserController(IUserService userService, IDistributedCache cache)
        {
            _userService = userService;
            _cache = cache;
        }

        /// <summary>
        /// Lấy tất cả người dùng
        /// </summary>
        /// <returns>Danh sách tất cả người dùng.</returns>
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                string cacheKey = "users:all";
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (cachedData != null)
                {
                    var users = JsonSerializer.Deserialize<object>(cachedData);
                    return Ok(HTTPResponse<object>.Response(200, "Fetched all users from cache.", users));
                }

                var usersFromDb = await _userService.GetAllUserAsync();

                var serialized = JsonSerializer.Serialize(usersFromDb);
                await _cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });

                return Ok(HTTPResponse<object>.Response(200, "Fetched all users successfully.", usersFromDb));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }

        /// <summary>
        /// Lấy thông tin người dùng theo ID
        /// </summary>
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                string cacheKey = $"user:{id}";
                var cached = await _cache.GetStringAsync(cacheKey);
                if (cached != null)
                {
                    var user = JsonSerializer.Deserialize<object>(cached);
                    return Ok(HTTPResponse<object>.Response(200, "User fetched from cache", user));
                }

                var userFromDb = await _userService.GetUserByIdAsync(id);
                if (userFromDb == null)
                    return NotFound(HTTPResponse<object>.Response(404, "User not found", null));

                var serialized = JsonSerializer.Serialize(userFromDb);
                await _cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });

                return Ok(HTTPResponse<object>.Response(200, "User fetched successfully", userFromDb));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }

        /// <summary>
        /// Lấy thông tin profile của người dùng đang đăng nhập
        /// </summary>
        [HttpGet("/profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                return Unauthorized(HTTPResponse<object>.Response(401, "UserId claim not found or invalid", null));

            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                    return NotFound(HTTPResponse<object>.Response(404, "User not found", null));

                return Ok(HTTPResponse<object>.Response(200, "User fetched successfully", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }
    }
}
