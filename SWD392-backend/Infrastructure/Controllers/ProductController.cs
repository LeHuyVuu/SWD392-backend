using AutoMapper;
using cybersoft_final_project.Models;
using cybersoft_final_project.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32.SafeHandles;
using SWD392_backend.Infrastructure.Services.ElasticSearchService;
using SWD392_backend.Infrastructure.Services.ProductService;
using SWD392_backend.Models;
using SWD392_backend.Models.Request;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IElasticSearchService _elasticsearchService;

        public ProductController(IProductService productService, IElasticSearchService elasticSearchService)
        {
            _productService = productService;
            _elasticsearchService = elasticSearchService;
        }

        /// <summary>
        /// Tìm kiếm sản phẩm sử dụng Elasticsearch với phân trang, sắp xếp và lọc.
        /// </summary>
        /// <param name="q">Từ khóa tìm kiếm (mặc định rỗng).</param>
        /// <param name="categoryId">ID danh mục để lọc sản phẩm (tùy chọn).</param>
        /// <param name="page">Số trang hiện tại (mặc định 1).</param>
        /// <param name="size">Số lượng sản phẩm mỗi trang (mặc định 10).</param>
        /// <param name="sortBy">Trường để sắp xếp (mặc định "createdAt")(Sort by latest). Các giá trị hợp lệ: createdAt , name, price.</param>
        /// <param name="sortOrder">Thứ tự sắp xếp: "asc" hoặc "desc" (mặc định "desc").</param>
        /// <returns>
        /// - 200 OK: Trả về danh sách sản phẩm phân trang (PagedResult&lt;ProductResponse&gt;).
        /// - 400 Bad Request: Nếu tham số không hợp lệ (page, size, sortBy, sortOrder).
        /// - 404 Not Found: Nếu không tìm thấy sản phẩm.
        /// - 500 Internal Server Error: Nếu xảy ra lỗi server.
        /// </returns>
        /// <remarks>
        /// Sử dụng Elasticsearch để tìm kiếm sản phẩm dựa trên từ khóa và danh mục, hỗ trợ phân trang, sắp xếp và lọc. DONE
        /// </remarks>
        [HttpGet("search")]
        public async Task<ActionResult<List<ProductResponse>>> SearchProduct(
            [FromQuery] string q = "",
            [FromQuery] int? categoryId = null,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string sortBy = "createdAt",
            [FromQuery] string sortOrder = "desc"
        )
        {
            var response = await _elasticsearchService.SearchAsync(q, categoryId, page, size, sortBy, sortOrder);

            if (response == null)
                return NotFound();
            else
                return Ok(response);
        }

        /// <summary>
        /// Lấy danh sách product có phân trang.
        /// </summary>
        /// <param name="page">Số trang hiện tại (mặc định là 1)</param>
        /// <param name="pageSize">Số lượng product trên mỗi trang (mặc định là 10)</param>
        /// <response code="200">Trả về danh sách product thành công</response>
        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetProducts(
            [FromQuery]int page = 1, 
            [FromQuery] int pageSize = 10
            )
        {
            var products = await _productService.GetPagedProductAsync(page, pageSize);
            
            return Ok(HTTPResponse<object>.Response(200, "Lấy list product thành công", products));
        }

        /// <summary>
        /// Lấy product dựa theo ID.
        /// </summary>
        /// <param name="id">ID của product</param>
        /// <response code="200">Trả về product thành công</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDetailResponse>> GetById(int id)
        {
            var products = await _productService.GetByIdAsync(id);
            if (products == null)
                return BadRequest(HTTPResponse<object>.Response(400, "Không có sản phầm trùng khớp", null));
            else
                return Ok(HTTPResponse<object>.Response(200, "Lấy sản phẩm thành công", products));
        }

        /// <summary>
        /// Lấy product dựa theo Slug.
        /// </summary>
        /// <param name="slug">Slug của product</param>
        /// <response code="200">Trả về product thành công</response>
        [HttpGet("{slug}")]
        public async Task<ActionResult<ProductDetailResponse>> GetBySlug(string slug)
        {
            var products = await _productService.GetBySlugAsync(slug);
            if (products == null)
                return BadRequest(HTTPResponse<object>.Response(400, "Không có sản phầm trùng khớp", null));
            else
                return Ok(HTTPResponse<object>.Response(200, "Lấy sản phẩm thành công", products));
        }

        /// <summary>
        /// Thêm mới product.
        /// </summary>
        /// <param name="request">Dữ liệu thêm mới cho sản phẩm.</param>
        /// <response code="200">Trả về nếu thêm mới thành công</response>
        [HttpPost("add")]
        public async Task<ActionResult<ProductResponse>> AddProduct([FromBody] AddProductRequest request)
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

                var result = await _productService.AddProductAsync(id, request);

                return Ok(HTTPResponse<object>.Response(200, "Thêm sản phẩm thành công", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }

        /// <summary>
        /// Cập nhật product.
        /// </summary>
        /// <param name="id">ID của sản phẩm cần cập nhật.</param>
        /// <param name="request">Dữ liệu cập nhật cho sản phẩm.</param>
        /// <response code="200">Trả về nếu cập nhật thành công</response>
        [HttpPost("update/{id}")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct(int id,[FromBody] UpdateProductRequest request)
        {
            var response = await _productService.UpdateProductAsync(id, request);

            if (response == null)
                return BadRequest(HTTPResponse<object>.Response(400, "Cập nhật sản phẩm thất bại", null));
            else
                return Ok(HTTPResponse<object>.Response(200, "Cập nhật sản phẩm thành công", response));
        }

        /// <summary>
        /// Ẩn/hiện product.
        /// </summary>
        /// <param name="id">ID của sản phẩm cần cập nhật.</param>
        /// <response code="200">Trả về nếu cập nhật thành công</response>
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> StatusProduct(int id, [FromBody] UpdateStatusProductRequest request)
        {
            var response = await _productService.UpdateProductStatusAsync(id, request);

            if (!response)
                return BadRequest(HTTPResponse<object>.Response(400, "Cập nhật sản phẩm thất bại", null));
            else
                return Ok(HTTPResponse<object>.Response(200, "Cập nhật sản phẩm thành công", response));
        }

        /// <summary>
        /// Xóa một sản phẩm dựa trên ID.
        /// </summary>
        /// <param name="id">ID của sản phẩm cần xóa (kiểu số nguyên).</param>
        /// <returns>
        /// - 204 No Content: Nếu sản phẩm được xóa thành công.
        /// - 400 Bad Request: Nếu ID không hợp lệ hoặc xóa thất bại.
        /// - 500 Internal Server Error: Nếu xảy ra lỗi server.
        /// </returns>
        /// <remarks>
        /// Gọi dịch vụ để xóa sản phẩm. Trả về thông báo thành công hoặc lỗi kèm chi tiết.
        /// </remarks>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productService.RemoveProductStatusAsync(id);

            if (!response)
                return BadRequest(HTTPResponse<object>.Response(400, "Xóa sản phẩm thất bại", response));
            else
                return Ok(HTTPResponse<object>.Response(200, "Xóa sản phẩm thành công", response));
        }
    }
}
