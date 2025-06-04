namespace SWD392_backend.Infrastructure.Services.SupplerSerivce;

public class SupplierService : ISupplierService
{
    
    private readonly IUnitOfWork _unitOfWork;


    public SupplierService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<int> GetTotalSuppliersAsync()
    {
        return await _unitOfWork.SupplierRepository.GetTotalCountAsync();
    }

}