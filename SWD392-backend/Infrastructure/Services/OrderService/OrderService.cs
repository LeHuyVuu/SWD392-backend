using cybersoft_final_project.Models.Request;
using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CheckoutAsync(OrderCheckoutDTO orderDTO)
    {
        // Lấy transaction và dùng using để tự dispose
        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var newOrder = new order()
            {
                id = Guid.NewGuid(),
                user_id = orderDTO.UserId,
                supplier_id = orderDTO.SupplierId,
                address = orderDTO.Address,
                shipping_price = orderDTO.ShippingPrice,
                total = orderDTO.Total,
                created_at = DateTime.UtcNow,
            };

            await _unitOfWork.OrderRepository.AddAsync(newOrder);

            foreach (var detail in orderDTO.OrderDetails)
            {
                var orderDetail = new orders_detail
                {
                    product_id = newOrder.id,
                    id = detail.ProductId,
                    quantity = detail.Quantity,
                    price = detail.Price,
                    discount_percent = detail.DiscountPercent,
                    note = detail.Note,
                    
                };
                await _unitOfWork.OrdersDetailRepository.AddAsync(orderDetail);
            }

            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

}
