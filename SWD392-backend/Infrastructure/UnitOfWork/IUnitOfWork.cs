using SWD392_backend.Infrastructure.Repositories.CategoryRepository;
using SWD392_backend.Infrastructure.Repositories.ProductRepository;
using SWD392_backend.Infrastructure.Repositories.UserRepository;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ICategoryRepository ProductImageRepository { get; }
    Task SaveAsync();
}