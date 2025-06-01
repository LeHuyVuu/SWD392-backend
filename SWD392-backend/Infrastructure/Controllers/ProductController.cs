using AutoMapper;
using cybersoft_final_project.Models;
using cybersoft_final_project.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32.SafeHandles;
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

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailResponse>> GetById(int id)
        {
            var products = await _productService.GetByIdAsync(id);
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
            var response = await _productService.AddProductAsync(request);

            if (response == null)
                return BadRequest(HTTPResponse<object>.Response(400, "Thêm sản phẩm thất bại", null));
            else
                return Ok(HTTPResponse<object>.Response(200, "Thêm sản phẩm thành công", response));
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
    }
}
