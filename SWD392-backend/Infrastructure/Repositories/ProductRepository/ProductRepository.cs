using Microsoft.EntityFrameworkCore;
using SWD392_backend.Context;
using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _context;

        public ProductRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<product>> GetAllProductAsync()
        {
            return await _context.products.ToListAsync();
        }
    }
}
