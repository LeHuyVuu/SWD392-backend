using Microsoft.EntityFrameworkCore;
using SWD392_backend.Context;
using SWD392_backend.Entities;
using SWD392_backend.Models;

namespace SWD392_backend.Infrastructure.Repositories.SupplierRepository;

public class SupplierRepository : ISupplierRepository
{
    private readonly MyDbContext _context;

    public SupplierRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<product>> GetPagedProductsAsync(int supplierId, int pageNumber, int pageSize)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;

        // Total Items
        var totalItems = await _context.products.Where(i => i.SupplierId == supplierId).CountAsync();

        var products = await _context.products
                            .Include(p => p.product_attributes)
                            .Include(p => p.product_images)
                            .Where(p => p.SupplierId == supplierId)
                            .OrderByDescending(p => p.CreatedAt)
                            .AsNoTracking()
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return new PagedResult<product>
        {
            Items = products,
            TotalItems = totalItems,
            Page = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<product?> GetProductByIdAsync(int id, int productId)
    {
        return await _context.products
                                .Include(p => p.categories)
                                .Include(p => p.supplier)
                                .Include(p => p.product_images)
                                .FirstOrDefaultAsync(p => p.Id == productId && p.SupplierId == id);
    }

    public async Task<supplier?> GetSupplierByIdAsync(int id)
    {
        return await _context.suppliers.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.suppliers.CountAsync();
    }
}

