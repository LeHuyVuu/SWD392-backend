using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.ProductRepository;

namespace SWD392_backend.Infrastructure.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<product>> GetAllProductAsync()
        {
            return await _productRepository.GetAllProductAsync();
        }
    }
}
