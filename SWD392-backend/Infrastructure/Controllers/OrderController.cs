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

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] OrderCheckoutDTO orderDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
             var result = await _orderService.CheckoutAsync(orderDTO);
            if (result)
                return Ok(new { message = "Order created successfully" });
            else
                return StatusCode(500, "Failed to create order");
        }
        catch (Exception ex)
        {
            // log ex nếu có
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
