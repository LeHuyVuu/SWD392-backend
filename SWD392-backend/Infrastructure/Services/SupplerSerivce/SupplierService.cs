using AutoMapper;
using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.SupplierRepository;
using SWD392_backend.Models;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.SupplerSerivce;

public class SupplierService : ISupplierService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISupplierRepository _supploerRepository;
    private readonly IMapper _mapper;


    public SupplierService(IUnitOfWork unitOfWork, ISupplierRepository supploerRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _supploerRepository = supploerRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductDetailResponse>> GetPagedProductsAsync(int supplierId, int pageNumber, int pageSize)
    {
        var pagedResult = await _supploerRepository.GetPagedProductsAsync(supplierId, pageNumber, pageSize);

        // Model Mapper
        var productDtos = _mapper.Map<List<ProductDetailResponse>>(pagedResult.Items);

        return new PagedResult<ProductDetailResponse>
        {
            Items = productDtos,
            TotalItems = pagedResult.TotalItems,
            Page = pagedResult.Page,
            PageSize = pagedResult.PageSize
        };
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