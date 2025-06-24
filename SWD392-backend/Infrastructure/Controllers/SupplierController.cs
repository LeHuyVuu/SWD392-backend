using cybersoft_final_project.Models;
using Elastic.Clients.Elasticsearch.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Infrastructure.Services.SupplerSerivce;
using SWD392_backend.Models;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// List products for SUPPLIER
        /// </summary>
        [HttpGet("products")]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetListProducts(int pageNumber = 1, int pageSize = 10)
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

                var result = await _supplierService.GetPagedProductsAsync(id, pageNumber, pageSize);

                return Ok(HTTPResponse<object>.Response(200, "Lấy danh sách sản phẩm thành công", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }

        /// <summary>
        /// Get product by ID for SUPPLIER
        /// </summary>
        [HttpGet("products/{productId:int}")]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetProductById(int productId)
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

                var result = await _supplierService.GetProductByIdAsync(id, productId);

                if (result == null)
                    return Ok(HTTPResponse<object>.Response(400, "Not Found", result));

                return Ok(HTTPResponse<object>.Response(200, "Lấy sản phẩm thành công", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }

        /// <summary>
        /// List order for supplier
        /// </summary>
        [HttpGet("orders")]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetListOrders(int pageNumber = 1, int pageSize = 10)
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

                var result = await _supplierService.GetPagedOrdersAsync(id, pageNumber, pageSize);

                if (result == null)
                    return Ok(HTTPResponse<object>.Response(400, "Not Found", result));

                return Ok(HTTPResponse<object>.Response(200, "Lấy danh sách đơn hàng  thành công", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }
    }
}
