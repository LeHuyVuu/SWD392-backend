using SWD392_backend.Entities;
using SWD392_backend.Models;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<PagedResult<ProductResponse>> GetPagedProductsAsync(int page, int pageSize);
        Task<product> GetByIdAsync(int id);
        Task AddAsync(product product);
        void Update(product product);
    }
}
