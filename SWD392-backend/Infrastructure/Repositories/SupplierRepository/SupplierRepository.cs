using Microsoft.EntityFrameworkCore;
using SWD392_backend.Context;
using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Repositories.SupplierRepository;

public class SupplierRepository : ISupplierRepository
{
    private readonly MyDbContext _context;

    public SupplierRepository(MyDbContext context)
    {
        _context = context;
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

