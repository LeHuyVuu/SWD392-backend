using System.Threading.Tasks;
using AutoMapper;
using cybersoft_final_project.Models.Request;
using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.ProductRepository;
using SWD392_backend.Models;
using SWD392_backend.Models.Request;
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

        public async Task<ProductDetailResponse> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            // Model mapper
            var response = _mapper.Map<ProductDetailResponse>(product);

            return response;
        }

        public async Task<ProductDetailResponse> GetBySlugAsync(string slug)
        {
            var product = await _productRepository.GetBySlugAsync(slug);

            // Model mapper
            var response = _mapper.Map<ProductDetailResponse>(product);

            return response;
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

        public async Task<ProductResponse> AddProductAsync(AddProductRequest request)
        {
            // Map from request
            var product = _mapper.Map<product>(request);

            // Add another field
            product.CreatedAt = DateTime.UtcNow;
            product.DiscountPrice = product.Price - (product.Price * product.DiscountPercent / 100);
            product.AvailableQuantity = product.StockInQuantity - product.SoldQuantity;
            product.IsActive = true;
            product.Slug = SlugHelper.Slugify(product.Name);
            product.SupplierId = request.SupplierId;

            // Insert
            await _productRepository.AddAsync(product);

            // Save
            await _unitOfWork.SaveAsync();

            var response = _mapper.Map<ProductResponse>(product);
            return response;
        }

        public async Task<ProductResponse> UpdateProductAsync(int id, UpdateProductRequest request)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            // Map into exist product
            _mapper.Map(request, product);

            product.DiscountPrice = product.Price - (product.Price * product.DiscountPercent / 100);
            product.AvailableQuantity = product.StockInQuantity - product.SoldQuantity;
            product.IsActive = true;
            product.Slug = SlugHelper.Slugify(product.Name);

            // Update
            _unitOfWork.ProductRepository.Update(product);

            // Save
            await _unitOfWork.SaveAsync();

            var response = _mapper.Map<ProductResponse>(product);

            return response;
        }
    }
}
