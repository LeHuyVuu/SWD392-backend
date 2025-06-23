using cybersoft_final_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SWD392_backend.Infrastructure.Services.OrderService;
using SWD392_backend.Infrastructure.Services.ShipperService;
using SWD392_backend.Models.Request;
using System.Text.Json;

namespace SWD392_backend.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;
        private readonly IOrderService _orderService;
        private readonly IDistributedCache _cache;

        public ShipperController(IShipperService shipperService, IOrderService orderService, IDistributedCache cache)
        {
            _shipperService = shipperService;
            _orderService = orderService;
            _cache = cache;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var role = User.FindFirst("Role")?.Value;

                if (string.IsNullOrEmpty(role))
                    return Unauthorized(HTTPResponse<object>.Response(401, "Role claim not found.", null));

                string? idClaimType = role == "SHIPPER" ? "UserId" : role == "SUPPLIER" ? "SupplierId" : null;

                if (idClaimType == null)
                    return Unauthorized(HTTPResponse<object>.Response(401, "Unsupported role.", null));

                var idClaim = User.FindFirst(idClaimType)?.Value;

                if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out int id))
                    return BadRequest(HTTPResponse<object>.Response(400, $"Invalid or missing {idClaimType}.", null));

                // Tạo key cache theo shipper + page
                string cacheKey = $"shipper:orders:{id}:page:{pageNumber}:size:{pageSize}";
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (cachedData != null)
                {
                    var result = JsonSerializer.Deserialize<object>(cachedData);
                    return Ok(HTTPResponse<object>.Response(200, "Fetched from cache", result));
                }

                var freshData = await _orderService.GetOrdersToShipper(id, pageNumber, pageSize);

                var serializedData = JsonSerializer.Serialize(freshData);
                await _cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

                return Ok(HTTPResponse<object>.Response(200, "Fetched successfully", freshData));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }

        [HttpPost("assign_area")]
        public async Task<IActionResult> AssignArea([FromBody] AssignAreaRequest request)
        {
            try
            {
                var role = User.FindFirst("Role")?.Value;

                if (string.IsNullOrEmpty(role))
                    return Unauthorized(HTTPResponse<object>.Response(401, "Role claim not found.", null));

                string? idClaimType = role == "SHIPPER" ? "UserId" : role == "SUPPLIER" ? "SupplierId" : null;

                if (idClaimType == null)
                    return Unauthorized(HTTPResponse<object>.Response(401, "Unsupported role.", null));

                var idClaim = User.FindFirst(idClaimType)?.Value;

                if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out int id))
                    return BadRequest(HTTPResponse<object>.Response(400, $"Invalid or missing {idClaimType}.", null));

                var result = await _shipperService.AssignAreaAsync(id, request);

                if (!result)
                    return BadRequest(HTTPResponse<object>.Response(400, "Something wrong", result));

                return Ok(HTTPResponse<object>.Response(200, "Update successfully", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }
    }
}
