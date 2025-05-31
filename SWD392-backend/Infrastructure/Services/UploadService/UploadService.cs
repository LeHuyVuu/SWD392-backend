using System.Threading.Tasks;
using SWD392_backend.Infrastructure.Services.CategoryService;
using SWD392_backend.Infrastructure.Services.S3Service;
using SWD392_backend.Models.Request;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.UploadService
{
    public class UploadService : IUploadService
    {
        private readonly IS3Service _s3Service;
        private readonly ICategoryService _categoryService;

        public UploadService(IS3Service s3Service, ICategoryService categoryService)
        {
            _s3Service = s3Service;
            _categoryService = categoryService;
        }

        public async Task<UploadMainProductImgResponse> UploadImage(UploadMainProductImgRequest request)
        {
            // Get category slug
            var categorySlug = await _categoryService.GetCategorySlugByIdAsync(request.CategoryId);

            var key = $"{categorySlug}/{request.ProductSlug}-{Guid.NewGuid()}-{request.ProductId}_{request.SupplierId}_1";

            var url = _s3Service.GeneratePreSignedURL(key, request.ContentType);

            // Generate image link
            var cdnDomain = Environment.GetEnvironmentVariable("CDN_DOMAIN");
            var imageUrl = $"{cdnDomain}/{key}";

            return new UploadMainProductImgResponse{ Url = url, Key = key, ImageUrl = imageUrl};
        }
    }
}
