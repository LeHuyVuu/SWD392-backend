using SWD392_backend.Infrastructure.Repositories.UserRepository;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    Task SaveAsync();
}