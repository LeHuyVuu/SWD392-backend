using Microsoft.EntityFrameworkCore;
using SWD392_backend.Context;

namespace SWD392_backend.Infrastructure.Repositories.SupplierRepository;

public class SupplierRepository : ISupplierRepository
{
    private readonly MyDbContext _context;

    public SupplierRepository(MyDbContext context)
    {
        _context = context;
    }


    public Task<int> GetTotalCountAsync()
    {
        return _context.suppliers.CountAsync();
    }
}

