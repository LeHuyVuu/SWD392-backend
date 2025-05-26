using cybersoft_final_project.Models.Request;

namespace SWD392_backend.Infrastructure.Services.OrderService;

public interface IOrderService
{
    Task<bool> CheckoutAsync(OrderCheckoutDTO orderDTO);

}