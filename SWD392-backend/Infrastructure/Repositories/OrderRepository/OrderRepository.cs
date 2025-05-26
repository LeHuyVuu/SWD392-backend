using SWD392_backend.Context;

namespace SWD392_backend.Infrastructure.Repositories.OrderRepository;

// OrderRepository.cs
using SWD392_backend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class OrderRepository : IOrderRepository
{
    private readonly MyDbContext _context;

    public OrderRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(order entity)
    {
        await _context.orders.AddAsync(entity);
    }

    // Các method khác nếu cần
}
