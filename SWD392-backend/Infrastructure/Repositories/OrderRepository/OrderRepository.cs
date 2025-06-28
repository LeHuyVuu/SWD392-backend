using SWD392_backend.Context;

namespace SWD392_backend.Infrastructure.Repositories.OrderRepository;

// OrderRepository.cs
using SWD392_backend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SWD392_backend.Models;
using SWD392_backend.Entities.Enums;

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
    
    
    public IQueryable<order> GetAll()
    {
        return _context.orders
            .Include(o => o.orders_details)
            .AsQueryable(); // Quan trọng để chuỗi LINQ hoạt động
    }

    public async Task<PagedResult<order>> GetOrdersToShipperAsync(string areaCode, int pageNumber, int pageSize)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 10 : pageSize;

        //Total items
        var totalItems = await _context.orders
                        .Where(o => o.AreaCode == areaCode)
                        .CountAsync();

        var orders = await _context.orders
                    .Include(o => o.user)
                    .Include(o => o.supplier)
                    .Include(o => o.orders_details)
                        .ThenInclude(od => od.product)
                            .ThenInclude(od => od.product_images)
                    .Where(o => o.AreaCode == areaCode)
                    .OrderByDescending(o => o.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        return new PagedResult<order>
        {
            Items = orders,
            TotalItems = totalItems,
            Page = pageNumber,
            PageSize = pageSize
        };
    }

    public orders_detail? GetOrdersDetail(string orderId, int productId)
    {
        return   _context.orders_details.FirstOrDefault(p => p.OrderId.ToString() == orderId && p.ProductId == productId);
  

        
    }

    public async Task<int> GetTotalOrdersAsync()
    {
        return await _context.orders.CountAsync();
    }



    // Các method khác nếu cần
}