using cybersoft_final_project.Models.Request;
using SWD392_backend.Models.Request;

namespace SWD392_backend.Infrastructure.Services.OrderService;

public interface IOrderService
{
    Task<bool> CheckoutAsync(OrderCheckoutDTO orderDTO, int userId);

    Task<object> GetOrdersByRoleAsync(string role, int id, int page, int pageSize);

    Task<bool> UpdateOrderItemStatusAsync(UpdateOrderItemStatus dto);
    
    
}