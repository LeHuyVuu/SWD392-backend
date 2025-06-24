using SWD392_backend.Entities;
using SWD392_backend.Models;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.SupplerSerivce;

public interface ISupplierService
{
    
    
    Task<int> GetTotalSuppliersAsync();
    Task<supplier> GetSupplierByIdAsync(int id);

    Task<PagedResult<ProductDetailResponse>> GetPagedProductsAsync(int supplierId, int pageNumber, int pageSize);
}