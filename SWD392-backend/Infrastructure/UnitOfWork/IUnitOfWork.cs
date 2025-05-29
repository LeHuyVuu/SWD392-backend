using SWD392_backend.Infrastructure.Repositories.CategoryRepository;
using SWD392_backend.Infrastructure.Repositories.ProductRepository;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using SWD392_backend.Infrastructure.Repositories.OrderDetailRepository;
using SWD392_backend.Infrastructure.Repositories.OrderRepository;
using SWD392_backend.Infrastructure.Repositories.UserRepository;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IProductRepository ProductRepository { get; }
    IOrderRepository OrderRepository { get; }
    IOrdersDetailRepository OrdersDetailRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ICategoryRepository ProductImageRepository { get; }
    Task SaveAsync();

    // Trả về transaction để có thể dùng using
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}