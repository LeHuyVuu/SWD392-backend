using AutoMapper;
using cybersoft_final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetProducts(int page = 1, int pageSize = 10)
        {
            var products = await _productService.GetPagedProductAsync(page, pageSize);
            
            return Ok(HTTPResponse<object>.Response(200, "Lấy list product thành công", products));
        }
    }
}
