using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.ProductRepository;
using SWD392_backend.Models;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.ProductService
{
    public interface IProductService
    {
        Task<PagedResult<ProductResponse>> GetPagedProductAsync(int page, int pageSize);
        Task<ProductResponse> GetByIdAsync(int id);
    }
}
