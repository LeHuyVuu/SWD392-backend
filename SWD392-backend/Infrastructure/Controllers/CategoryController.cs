using cybersoft_final_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Services.CategoryService;
using SWD392_backend.Models.Response;


namespace SWD392_backend.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponse>>> GetCategories()
        {
            var response = await _categoryService.GetCategoriesAsync();

            if (response == null)
                return BadRequest(HTTPResponse<object>.Response(400, "Không tìm thấy category nào", null));

            return Ok(HTTPResponse<List<CategoryResponse>>.Response(200, "Lấy toàn bộ category thành công", response));
        }
    }
}
