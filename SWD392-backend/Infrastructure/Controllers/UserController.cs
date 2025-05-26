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

        [HttpGet]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUserAsync();
            return Ok(users);
        }
        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            return Ok(user);
        }

    }
}
