using SWD392_backend.Context;

namespace SWD392_backend.Infrastructure.Repositories.OrderDetailRepository;

// OrdersDetailRepository.cs
using SWD392_backend.Entities;
using System.Threading.Tasks;

public class OrdersDetailRepository : IOrdersDetailRepository
{
    private readonly MyDbContext _context;

    public OrdersDetailRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(orders_detail entity)
    {
        await _context.orders_details.AddAsync(entity);
    }

    // Các method khác nếu cần
}
