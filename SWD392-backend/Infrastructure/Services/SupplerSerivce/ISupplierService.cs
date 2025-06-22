using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Services.SupplerSerivce;

public interface ISupplierService
{
    
    
    Task<int> GetTotalSuppliersAsync();
    Task<supplier> GetSupplierByIdAsync(int id);
}