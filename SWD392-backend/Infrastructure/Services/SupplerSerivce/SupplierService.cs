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

    public async Task<OrderResponse> GetOrderByIdAsync(int id, Guid orderId)
    {
        var supplier = await _supploerRepository.GetSupplierByIdAsync(id);       
        if (supplier == null)
        {
            Console.WriteLine("SUPPLIER NOT FOUND");
            return null;
        }
            

        var order = await _supploerRepository.GetOrderByIdAsync(id, orderId);
        if (order == null)
        {
            Console.WriteLine("ORDER NOT FOUND");
            return null;
        }


        var orderDto = _mapper.Map<OrderResponse>(order);
        return orderDto;
    }

    public async Task<PagedResult<OrderResponse>> GetPagedOrdersAsync(int supplierId, int pageNumber, int pageSize)
    {
        var supplier = await _supploerRepository.GetSupplierByIdAsync(supplierId);
        if (supplier == null)
            return null;

        var pagedResult = await _supploerRepository.GetPagedOrdersAsync(supplierId, pageNumber, pageSize);

        var orderDtos = _mapper.Map<List<OrderResponse>>(pagedResult.Items);

        return new PagedResult<OrderResponse>
        {
            Items = orderDtos,
            TotalItems = pagedResult.TotalItems,
            Page = pagedResult.Page,
            PageSize = pagedResult.PageSize
        };
    }

    public async Task<PagedResult<ProductResponse>> GetPagedProductsAsync(int supplierId, int pageNumber, int pageSize)
    {
        var supplier = await _supploerRepository.GetSupplierByIdAsync(supplierId);
        if (supplier == null)
            return null;

        var pagedResult = await _supploerRepository.GetPagedProductsAsync(supplierId, pageNumber, pageSize);

        // Model Mapper
        var productDtos = _mapper.Map<List<ProductResponse>>(pagedResult.Items);

        return new PagedResult<ProductResponse>
        {
            Items = productDtos,
            TotalItems = pagedResult.TotalItems,
            Page = pagedResult.Page,
            PageSize = pagedResult.PageSize
        };
    }

    public async Task<ProductDetailResponse> GetProductByIdAsync(int id, int productId)
    {
        var supplier = await _supploerRepository.GetSupplierByIdAsync(id);
        if (supplier == null)
            return null;

        var product = await _supploerRepository.GetProductByIdAsync(id, productId);

        if (product == null)
            return null;

        var productDto = _mapper.Map<ProductDetailResponse>(product);

        return productDto;
    }

    public async Task<product> GetProductToRemoveAsync(int id, int productId)
    {
        var supplier = await _supploerRepository.GetSupplierByIdAsync(id);
        if (supplier == null)
            return null;

        var product = await _supploerRepository.GetProductByIdAsync(id, productId);

        if (product == null)
            return null;

        return product;
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