using SWD392_backend.Entities;
using SWD392_backend.Models;

namespace SWD392_backend.Infrastructure.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<PagedResult<product>> GetPagedProductsAsync(int page, int pageSize, string sortBy = "Id", string sortOrder = "asc");
    }
}
