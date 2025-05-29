using AutoMapper;
using cybersoft_final_project.Models.Request;
using Microsoft.EntityFrameworkCore;
using SWD392_backend.Entities;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<bool> CheckoutAsync(OrderCheckoutDTO orderDTO, int userId)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var newOrder = new order()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                SupplierId = orderDTO.SupplierId,
                Address = orderDTO.Address,
                ShippingPrice = orderDTO.ShippingPrice,
                Total = orderDTO.Total,
                CreatedAt = DateTime.UtcNow,
            };

            await _unitOfWork.OrderRepository.AddAsync(newOrder);
            await _unitOfWork.SaveAsync();

            foreach (var detail in orderDTO.OrderDetails)
            {
                // Lấy product theo id
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(detail.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product with ID {detail.ProductId} not found.");

                // Kiểm tra tồn kho đủ
                if (product.StockInQuantity < detail.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for product ID {detail.ProductId}.");

                // Trừ tồn kho
                product.StockInQuantity -= detail.Quantity;

                // Cập nhật product trong DB
                _unitOfWork.ProductRepository.Update(product);

                // Thêm chi tiết đơn hàng
                var orderDetail = new orders_detail
                {
                    OrderId = newOrder.Id,
                    ProductId = detail.ProductId,
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
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    
    public async Task<object> GetAllOrdersAsync(int userId, int page, int pageSize)
    {
        var query = _unitOfWork.OrderRepository
            .GetAll()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt);

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var orders = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var orderDTOs = _mapper.Map<List<OrderResponse>>(orders);

        return new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Items = orderDTOs
        };
    }
}