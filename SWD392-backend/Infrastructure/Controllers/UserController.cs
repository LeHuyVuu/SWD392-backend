using cybersoft_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Infrastructure.Services.UserService;

namespace SWD392_backend.Infrastructure.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Lấy tất cả người dùng
        /// </summary>
        /// <returns>Danh sách tất cả người dùng.</returns>
        /// <response code="200">Trả về danh sách người dùng thành công.</response>
        /// <response code="500">Lỗi hệ thống khi lấy dữ liệu người dùng.</response>
        [HttpGet]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var users = await _userService.GetAllUserAsync();

                return Ok(HTTPResponse<object>.Response(200, "Fetched all users successfully.", users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }

        /// <summary>
        /// Lấy thông tin người dùng theo ID
        /// </summary>
        /// <param name="id">ID người dùng cần lấy thông tin.</param>
        /// <returns>Thông tin người dùng.</returns>
        /// <response code="200">Trả về thông tin người dùng thành công.</response>
        /// <response code="404">Không tìm thấy người dùng với ID đã cho.</response>
        /// <response code="500">Lỗi hệ thống khi lấy dữ liệu người dùng.</response>
        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(HTTPResponse<object>.Response(404, "User not found", null));
                }

                return Ok(HTTPResponse<object>.Response(200, "User fetched successfully", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }
    }
}
