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

        /// <summary>
        /// Lấy danh sách tất cả các danh mục (categories).
        /// </summary>
        /// <returns>Danh sách các category dưới dạng mảng JSON.</returns>
        /// <response code="200">Trả về danh sách category thành công.</response>
        /// <response code="400">Không tìm thấy category nào hoặc có lỗi trong quá trình xử lý.</response>
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
