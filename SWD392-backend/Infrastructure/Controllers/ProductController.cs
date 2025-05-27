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

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetById(int id)
        {
            var products = await _productService.GetByIdAsync(id);
            if (products == null)
                return BadRequest(HTTPResponse<object>.Response(404, "Không có sản phầm trùng khớp", null));
            else
                return Ok(HTTPResponse<object>.Response(200, "Lấy sản phẩm thành công", products));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateProductRequest request)
        {
            bool checkUpdate = await _productService.UpdateProductAsync(id, request);

            if (!checkUpdate)
                return BadRequest(HTTPResponse<object>.Response(404, "Update không thành công", null));
            else
                return Ok(HTTPResponse<object>.Response(200, "Update thành công", null));
        }
    }
}
