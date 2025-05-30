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
                Id = Guid.NewGuid(),
                UserId = orderDTO.UserId,
                SupplierId = orderDTO.SupplierId,
                Address = orderDTO.Address,
                ShippingPrice = orderDTO.ShippingPrice,
                Total = orderDTO.Total,
                CreatedAt = DateTime.UtcNow,
            };

            await _unitOfWork.OrderRepository.AddAsync(newOrder);

            foreach (var detail in orderDTO.OrderDetails)
            {
                var orderDetail = new orders_detail
                {
                    OrderId = newOrder.Id,
                    Id = detail.ProductId,
                    Quantity = detail.Quantity,
                    Price = detail.Price,
                    DiscountPercent = detail.DiscountPercent,
                    Note = detail.Note,
                    
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
