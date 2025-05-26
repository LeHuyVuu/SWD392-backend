using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Infrastructure.Services.ProductService;

namespace SWD392_backend.Infrastructure.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _productService.GetAllProductAsync();
            return Ok(products);
        }
    }
}
