using cybersoft_final_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SWD392_backend.Infrastructure.Services.ProductImageService;
using SWD392_backend.Models.Request;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpPost("add/{productId}")]
        public async Task<ActionResult<List<ProductImageResponse>>> AddProductImage(int productId, [FromBody] List<ProductImageRequest> request)
        {
            // Check main
            if (request.Count(img => img.IsMain) > 1)
                return BadRequest(HTTPResponse<object>.Response(400, "Chỉ tồn tại 1 hình ảnh là is_main", null));

            var response = await _productImageService.AddProductImageAsync(productId, request);

            if (response == null) 
                return BadRequest(HTTPResponse<object>.Response(400, "Thêm hình ảnh cho sản phẩm thất bại", response));
            return Ok(HTTPResponse<object>.Response(200, "Thêm hình ảnh cho sản phẩm thành công", response));
        }
    }
}
