namespace SWD392_backend.Infrastructure.Repositories.SupplierRepository;

public interface ISupplierRepository
{
    Task<int> GetTotalCountAsync();
}