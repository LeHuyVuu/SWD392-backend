using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Repositories.SupplierRepository;

public interface ISupplierRepository
{
    Task<int> GetTotalCountAsync();
    Task<supplier> GetSupplierByIdAsync(int id);
}