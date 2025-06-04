using cybersoft_final_project.Models;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Services;
using SWD392_backend.Infrastructure.Services.OrderService;
using SWD392_backend.Infrastructure.Services.SupplerSerivce;
using SWD392_backend.Infrastructure.Services.UserService;

namespace SWD392_backend.Infrastructure.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly ISupplierService _supplierService;

        public DashboardController(IUserService userService, IOrderService orderService, ISupplierService supplierService)
        {
            _userService = userService;
            _orderService = orderService;
            _supplierService = supplierService;
        }

        /// <summary>
        /// Lấy số liệu thống kê về đơn hàng, người dùng và nhà cung cấp.
        /// </summary>
        /// <returns>Thông tin tổng quan cho dashboard.</returns>
        /// <response code="200">Trả về dữ liệu tổng quan thành công.</response>
        /// <response code="500">Lỗi hệ thống khi lấy dữ liệu tổng quan.</response>
        [HttpGet("overview")]
        public async Task<IActionResult> GetDashboardOverview()
        {
            try
            {
                // Lấy số lượng đơn hàng
                var totalOrders = await _orderService.GetTotalOrdersAsync();

                // Lấy số lượng người dùng
                var totalUsers = await _userService.GetTotalUserAsync();

                // Lấy số lượng nhà cung cấp
                var totalSuppliers = await _supplierService.GetTotalSuppliersAsync();

                // Chuẩn bị dữ liệu trả về
                var dashboardData = new
                {
                    TotalOrders = totalOrders,
                    TotalUsers = totalUsers,
                    TotalSuppliers = totalSuppliers
                };

                return Ok(HTTPResponse<object>.Response(200, "Fetched dashboard data successfully.", dashboardData));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }
    }
}
