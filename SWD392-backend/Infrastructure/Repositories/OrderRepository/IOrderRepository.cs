namespace SWD392_backend.Infrastructure.Repositories.OrderRepository;

// IOrderRepository.cs
using SWD392_backend.Entities;
using SWD392_backend.Models;
using System;
using System.Threading.Tasks;

public interface IOrderRepository
{
    Task AddAsync(order entity);
    // Thêm các method cần thiết khác nếu muốn, ví dụ GetById, Update...
    IQueryable<order> GetAll();

    Task<int> GetTotalOrdersAsync();

    Task<PagedResult<order>> GetOrdersToShipperAsync(string areaCode, int pageNumber, int pageSize);

    orders_detail GetOrdersDetail(string orderId, int productId);

    Task<int> CountOrdersByMonthAsync(DateTime startDate, DateTime endDate);
    Task<PagedResult<order>> GetOrdersByMonthAsync(DateTime startDate, DateTime endDate, int pageNumber, int pageSize);

    Task<int> CountOrdersByDayAsync(DateTime startDate, DateTime endDate);

    Task<int> CountOrdersInRangeAsync(DateTime startDate, DateTime endDate);
    Task<int> CountNewUsersInRangeAsync(DateTime startDate, DateTime endDate);
}