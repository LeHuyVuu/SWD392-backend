using SWD392_backend.Context;
using SWD392_backend.Infrastructure.Repositories.UserRepository;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyDbContext _context;
    public IUserRepository UserRepository { get; }

    public UnitOfWork(MyDbContext context, IUserRepository userRepository)
    {
        _context = context;
        UserRepository = userRepository;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}