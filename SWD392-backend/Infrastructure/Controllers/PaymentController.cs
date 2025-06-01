using cybersoft_final_project.Models.Request;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Infrastructure.Services.OrderService;

namespace SWD392_backend.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;
    private readonly IOrderService _orderService;

    public PaymentController(PaymentService paymentService, IOrderService  orderService)
    {
        _paymentService = paymentService;
        _orderService = orderService;
    }

    [HttpPost("paypal")]
    public async Task<IActionResult> CreatePaypalOrder([FromBody] string totalPrice)
    {
        var approvalUrl = await _paymentService.CreateOrderAsync(totalPrice);
        return Ok(new { url = approvalUrl });
    }
    
    
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] OrderCheckoutDTO orderDTO)
    {
        // Lấy userId từ claim, nếu không có trả về 401 Unauthorized
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(new { message = "UserId claim not found. Unauthorized." });

        if (!int.TryParse(userIdClaim, out int userId))
            return BadRequest(new { message = "Invalid UserId claim format." });

        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid request data.", errors = ModelState });

        try
        {
            var result = await _orderService.CheckoutAsync(orderDTO, userId);
            if (result)
                return Ok(new { message = "Order created successfully" });
            else
                return StatusCode(500, new { message = "Failed to create order" });
        }
        catch (ArgumentException argEx)
        {
            // Ví dụ lỗi về dữ liệu đầu vào không hợp lệ
            return BadRequest(new { message = argEx.Message });
        }
        catch (KeyNotFoundException keyEx)
        {
            // Ví dụ lỗi do foreign key không tồn tại (sản phẩm, user, supplier ...)
            return NotFound(new { message = keyEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", detail = ex.ToString() });
        }
    }
}
