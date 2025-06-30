using Elastic.Clients.Elasticsearch.Inference;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Infrastructure.Services.OrderService;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public ReportController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("ordersbymonth")]
        public async Task<IActionResult> GetOrdersByMonth([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var count = await _orderService.CountOrdersByMonthAsync(month, year);
                return Ok(new
                {
                    year,
                    month,
                    totalOrders = count
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("ordersbyday")]
        public async Task<IActionResult> GetOrdersByMonth([FromQuery] int day, [FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var count = await _orderService.CountOrdersByDayAsync(day, month, year);
                return Ok(new
                {
                    year,
                    month,
                    day,
                    totalOrders = count
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("summarythismonth")]
        public async Task<IActionResult> SummaryThisMonth()
        {
            try
            {
                (int totalOrders, int totalUsers) = await _orderService.GetSummaryThisMonthAsync();
                return Ok(new
                {
                    totalOrders,
                    totalUsers
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}
