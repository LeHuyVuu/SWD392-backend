using cybersoft_final_project.Models;
using cybersoft_final_project.Models.Request;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Entities.Enums;
using SWD392_backend.Infrastructure.Services.OrderService;
using SWD392_backend.Models.Request;

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


    /// <summary>
    /// Lấy danh sách đơn hàng của người dùng hiện tại.
    /// </summary>
    /// <param name="page">Trang hiện tại (mặc định là 1).</param>
    /// <param name="pageSize">Số lượng đơn hàng mỗi trang (mặc định là 10).</param>
    /// <returns>Danh sách đơn hàng thuộc về tài khoản đăng nhập, có phân trang.</returns>
    /// <response code="200">Trả về danh sách đơn hàng của người dùng.</response>
    /// <response code="400">Sai định dạng tham số hoặc UserId không hợp lệ.</response>
    /// <response code="401">Người dùng chưa đăng nhập hoặc không có quyền.</response>
    /// <response code="500">Lỗi hệ thống khi truy xuất đơn hàng.</response>
    [HttpGet()]
    public async Task<IActionResult> GetOrdersByRole([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var role = User.FindFirst("Role")?.Value;

            if (string.IsNullOrEmpty(role))
                return Unauthorized(HTTPResponse<object>.Response(401, "Role claim not found.", null));

            string? idClaimType = role == "CUSTOMER" ? "UserId" : role == "SUPPLIER" ? "SupplierId" : null;

            if (idClaimType == null)
                return Unauthorized(HTTPResponse<object>.Response(401, "Unsupported role.", null));

            var idClaim = User.FindFirst(idClaimType)?.Value;

            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out int id))
                return BadRequest(HTTPResponse<object>.Response(400, $"Invalid or missing {idClaimType}.", null));

            var result = await _orderService.GetOrdersByRoleAsync(role, id, page, pageSize);
        
            return Ok(HTTPResponse<object>.Response(200, "Fetched orders successfully.", result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
        }
    }


    /// <summary>
    /// Cập nhật trạng thái của một mục đơn hàng (order item).
    /// </summary>
    /// <param name="dto">Đối tượng chứa thông tin cần thiết để cập nhật trạng thái (OrderDetailId, OrderId, ProductId, NewStatus).</param>
    /// <returns>Trả về kết quả cập nhật trạng thái.
    /// Nếu thành công trả về thông báo cập nhật thành công.
    /// Nếu trạng thái không thay đổi trả về thông báo không cần cập nhật.</returns>
    /// <response code="200">Cập nhật trạng thái thành công hoặc trạng thái không thay đổi.</response>
    /// <response code="404">Không tìm thấy mục đơn hàng tương ứng để cập nhật.</response>
    /// <response code="500">Lỗi hệ thống trong quá trình cập nhật trạng thái.</response>
    [HttpPut]
    public async Task<IActionResult> UpdateOrderItemStatus([FromBody] UpdateOrderItemStatus dto)
    {
        try
        {
            var updated = await _orderService.UpdateOrderItemStatusAsync(dto);

            if (updated)
                return Ok(HTTPResponse<object>.Response(200, "Order item status updated successfully.", null));
            else
                return Ok(HTTPResponse<object>.Response(200, "No change needed. Status is already up to date.", null));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(HTTPResponse<object>.Response(404, ex.Message, null));
        }
        catch (Exception ex)
        {
            return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
        }
    }



    [HttpGet("status")]
    public IActionResult GetEnumStatus()
    {
        try
        {
            var role = User.FindFirst("Role")?.Value;

            if (string.IsNullOrEmpty(role))
                return Unauthorized(HTTPResponse<object>.Response(401, "Role claim not found.", null));

            // Danh sách status cho CUSTOMER
            List<OrderStatus> customerStatuses = new List<OrderStatus>
            {
                OrderStatus.Pending,
                OrderStatus.Preparing,
                OrderStatus.Delivery,
                OrderStatus.Delivered,
                OrderStatus.Cancelled,
                OrderStatus.Refunding,
                OrderStatus.Refunded
            };

            // Danh sách status cho SHIPPER
            List<OrderStatus> shipperStatuses = new List<OrderStatus>
            {
                OrderStatus.Preparing,
                OrderStatus.Delivery,
                OrderStatus.Delivered
            };

            // Danh sách status cho SUPPLIER
            List<OrderStatus> supplierStatuses = new List<OrderStatus>
            {
                OrderStatus.Pending,
                OrderStatus.Preparing,
                OrderStatus.Cancelled,
                OrderStatus.Refunding
            };

            // Tạo danh sách status dựa trên role
            List<OrderStatus> allowedStatuses;

            if (role == "CUSTOMER")
            {
                allowedStatuses = customerStatuses;
            }
            else if (role == "SHIPPER")
            {
                allowedStatuses = shipperStatuses;
            }
            else if (role == "SUPPLIER")
            {
                allowedStatuses = supplierStatuses;
            }
            else
            {
                return Unauthorized(HTTPResponse<object>.Response(401, "Unsupported role.", null));
            }

            // Trả về danh sách status phù hợp dưới dạng chuỗi
            var result = allowedStatuses.Select(s => s.ToString()).ToList();

            return Ok(HTTPResponse<List<string>>.Response(200, "Fetched statuses successfully.", result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
        }
    }
}