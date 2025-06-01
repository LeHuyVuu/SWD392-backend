using cybersoft_final_project.Models.Request;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Infrastructure.Services.OrderService;

namespace SWD392_backend.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }




    [HttpGet]
    public async Task<IActionResult> GetAllOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(new { message = "UserId claim not found. Unauthorized." });

        if (!int.TryParse(userIdClaim, out int userId))
            return BadRequest(new { message = "Invalid UserId claim format." });

        if (page <= 0 || pageSize <= 0)
            return BadRequest(new { message = "Page and PageSize must be greater than 0." });

        try
        {
            var pagedResult = await _orderService.GetAllOrdersAsync(userId, page, pageSize);
            return Ok(pagedResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }
    }

    
}