using Microsoft.EntityFrameworkCore;
using SWD392_backend.Context;
using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Repositories.ProductImageRepository
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly MyDbContext _context;

        public ProductImageRepository(MyDbContext context)
        {
            _context = context;
        }

        public Task AddImages(List<product_image> images)
        {
            return _context.product_images.AddRangeAsync(images);
        }

        public async Task<List<product_image>> FindAllMainImage(int productId)
        {
            return await _context.product_images
                        .Where(img => img.ProductsId == productId && img.IsMain)
                        .ToListAsync();
        }
    }
}
