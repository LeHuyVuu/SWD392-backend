using System.Threading.Tasks;
using AutoMapper;
using cybersoft_final_project.Models.Request;
using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.ProductRepository;
using SWD392_backend.Models;
using SWD392_backend.Models.Response;
using SWD392_backend.Utilities;

namespace SWD392_backend.Infrastructure.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponse> GetByIdAsync(int id)
        {
            var products = await _productRepository.GetByIdAsync(id);

            // Model mapper
            var productDtos = _mapper.Map<ProductResponse>(products);

            return productDtos;
        }

        public async Task<PagedResult<ProductResponse>> GetPagedProductAsync(int page, int pageSize)
        {
            var pagedResult = await _productRepository.GetPagedProductsAsync(page, pageSize);

            // Model mapper
            var productDtos = _mapper.Map<List<ProductResponse>>(pagedResult.Items);


            return new PagedResult<ProductResponse>
            {
                Items = productDtos,
                TotalItems = pagedResult.TotalItems,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize
            };
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductRequest request)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return false;

            // Map into exist product
            _mapper.Map(request, product);

            product.DiscountPrice = product.Price - (product.Price * product.DiscountPercent / 100);
            product.AvailableQuantity = product.StockInQuantity - product.SoldQuantity;
            product.IsActive = true;
            product.Slug = SlugHelper.Slugify(product.Name);
            product.SupplierId = 5;

            // Update
            _unitOfWork.ProductRepository.Update(product);

            // Save
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
