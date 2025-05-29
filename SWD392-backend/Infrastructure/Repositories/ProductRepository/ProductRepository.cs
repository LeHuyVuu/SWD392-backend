using Microsoft.EntityFrameworkCore;
using SWD392_backend.Context;
using SWD392_backend.Entities;
using SWD392_backend.Models;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _context;

        public ProductRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(product product)
        {
            await _context.products.AddAsync(product);
        }

        public async Task<product?> GetByIdAsync(int id)
        {
            return await _context.products
                                .Include(p => p.categories)
                                .Include(p => p.supplier)
                                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResult<ProductResponse>> GetPagedProductsAsync(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            // Total items
            var totalItems = await _context.products.CountAsync();

            var products = await _context.products
                            .OrderBy(p => p.Id)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Select(p => new ProductResponse { 
                                     Id = p.Id,
                                     Name = p.Name,
                                     Price = p.Price,
                                     DiscountPrice = p.DiscountPrice,
                                     Slug = p.Slug,
                                     RatingAverage = p.RatingAverage,
                                     IsSale = p.IsSale,
                                     StockInQuantity = p.StockInQuantity,
                                     ImageUrl = p.product_images
                                                .Where(i => i.IsMain)
                                                .Select(i => i.ProductImageUrl)
                                                .FirstOrDefault() ?? "https://via.placeholder.com/150"
                            })
                            .ToListAsync();

            return new PagedResult<ProductResponse>
            {
                Items = products,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        public void Update(product product)
        {
            _context.products.Update(product);
        }
    }
}
