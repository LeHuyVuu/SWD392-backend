using SWD392_backend.Context;
using SWD392_backend.Infrastructure.Repositories.CategoryRepository;
using SWD392_backend.Infrastructure.Repositories.ProductRepository;
using SWD392_backend.Infrastructure.Repositories.UserRepository;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyDbContext _context;
    public IUserRepository UserRepository { get; }

    public IProductRepository ProductRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public ICategoryRepository ProductImageRepository { get; }

    public UnitOfWork(MyDbContext context, IUserRepository userRepository, IProductRepository productRepository, ICategoryRepository categoryRepository, ICategoryRepository productImageRepository)
    {
        _context = context;
        UserRepository = userRepository;
        ProductRepository = productRepository;
        CategoryRepository = categoryRepository;
        ProductImageRepository = productImageRepository;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}