using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.ProductRepository;

namespace SWD392_backend.Infrastructure.Services.ProductService
{
    public interface IProductService
    {
        Task<List<product>> GetAllProductAsync();
    }
}
