using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.SupplierRepository;

namespace SWD392_backend.Infrastructure.Services.SupplerSerivce;

public class SupplierService : ISupplierService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISupplierRepository _supploerRepository;


    public SupplierService(IUnitOfWork unitOfWork, ISupplierRepository supploerRepository)
    {
        _unitOfWork = unitOfWork;
        _supploerRepository = supploerRepository;
    }

    public async Task<supplier> GetSupplierByIdAsync(int id)
    {
        return await _supploerRepository.GetSupplierByIdAsync(id);
    }

    public async Task<int> GetTotalSuppliersAsync()
    {
        return await _unitOfWork.SupplierRepository.GetTotalCountAsync();
    }

}