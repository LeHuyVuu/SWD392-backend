using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<List<product>> GetAllProductAsync();
    }
}
