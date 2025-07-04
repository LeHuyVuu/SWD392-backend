﻿using cybersoft_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SWD392_backend.Infrastructure.Services.UserService;
using System.Text.Json;
using Elastic.Clients.Elasticsearch.Security;
using SWD392_backend.Models.Requests;

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

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.AddUserAsync(request);
                return Ok(new { message = "Người dùng đã được thêm thành công." });
            }
            catch (Exception ex)
            {
                // Log lỗi tại đây nếu có hệ thống logging
                return StatusCode(500, new { error = ex.Message });
            }
        }
        /// <summary>
        /// Lấy tổng số người dùng đã đăng ký trong một tháng cụ thể của một năm.
        /// </summary>
        /// <param name="month">Tháng cần thống kê (từ 1 đến 12).</param>
        /// <param name="year">Năm cần thống kê (lớn hơn 2000, nhỏ hơn hoặc bằng năm hiện tại).</param>
        /// <returns>
        /// Trả về tổng số người dùng đã đăng ký trong tháng và năm tương ứng.
        /// </returns>

        [HttpGet("usersbymonth")]
        public async Task<IActionResult> GetTotalUsersInMonth([FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12 || year > DateTime.Now.Year)
            {
                return BadRequest(HTTPResponse<object>.Response(400, "Tháng hoặc năm không hợp lệ.", null));
            }

            int total = await _userService.GetTotalUsersByMonth(month, year);

            var result = new
            {
                Month = month,
                Year = year,
                Total = total
            };

            return Ok(HTTPResponse<object>.Response(200, "Lấy tổng người dùng theo tháng thành công", result));
        }
        /// <summary>
        /// Lấy tổng số người dùng đã đăng ký trong một ngày cụ thể.
        /// </summary>
        /// <param name="day">Ngày cần thống kê (định dạng yyyy-MM-dd, ví dụ: 2025-06-30).</param>
        /// <returns>
        /// Trả về tổng số người dùng đã đăng ký đúng vào ngày đó.
        /// </returns>

        [HttpGet("usersbyday")]
        public async Task<IActionResult> GetUserCountByExactDay([FromQuery] DateTime day)
        {
            if (day == default)
                return BadRequest(HTTPResponse<object>.Response(400, "Ngày không hợp lệ", null));

            var total = await _userService.GetUserCountByExactDay(day);

            return Ok(HTTPResponse<object>.Response(200, "Thành công", new
            {
                date = day.ToString("yyyy-MM-dd"),
                total
            }));
        }



    }
}
