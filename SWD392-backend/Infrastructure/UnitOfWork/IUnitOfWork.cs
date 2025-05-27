using SWD392_backend.Infrastructure.Repositories.ProductRepository;
using SWD392_backend.Infrastructure.Repositories.UserRepository;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IProductRepository ProductRepository { get; }
    Task SaveAsync();
}