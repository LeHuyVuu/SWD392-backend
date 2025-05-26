using Microsoft.EntityFrameworkCore;
using SWD392_backend.Context;
using SWD392_backend.Entities;
using SWD392_backend.Models;

namespace SWD392_backend.Infrastructure.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _context;

        public ProductRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<product>> GetPagedProductsAsync(int page, int pageSize, string sortBy = "Id", string sortOrder = "asc")
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            // Total items
            var totalItems = await _context.products.CountAsync();

            var products = await _context.products
                            .Include(p => p.categories)
                            .Include(p => p.supplier)
                            .OrderBy(p => p.id)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return new PagedResult<product>
            {
                Items = products,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
