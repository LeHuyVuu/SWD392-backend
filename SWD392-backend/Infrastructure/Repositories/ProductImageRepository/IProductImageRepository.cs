using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Repositories.ProductImageRepository
{
    public interface IProductImageRepository
    {
        Task AddImages(List<product_image> images);
        Task<List<product_image>> FindAllMainImage(int productId);
    }
}
