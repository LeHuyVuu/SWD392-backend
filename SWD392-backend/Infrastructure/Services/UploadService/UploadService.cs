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

        public async Task<UploadMultipleProductImgsResponse> UploadMultipleImage(UploadProductImgsRequest request)
        {
            var categorySlug = await _categoryService.GetCategorySlugByIdAsync(request.CategoryId);
            var uploads = new List<UploadProductImgResponse>();
            var cdnDomain = Environment.GetEnvironmentVariable("CDN_DOMAIN");

            for (int i = 0; i < request.ContentTypes.Count; i++)
            {
                var contentType = request.ContentTypes[i].Trim();
                var extension = contentType switch
                {
                    "image/jpeg" => "jpg",
                    "image/png" => "png",
                    "image/gif" => "gif",
                    "image/webp" => "webp",
                    "image/bmp" => "bmp",
                    "image/svg+xml" => "svg",
                    "image/avif" => "avif",
                    _ => "img"
                };
                var key = $"{categorySlug}/{request.ProductSlug}-{Guid.NewGuid()}-{request.ProductId}_{request.SupplierId}_{i+1}.{extension}";
                var url = _s3Service.GeneratePreSignedURL(key, contentType);

                // Generate image link
                var imageUrl = $"{cdnDomain}/{key}";

                //Add to List
                uploads.Add(new UploadProductImgResponse
                {
                    Url = url,
                    Key = key,
                    ImageUrl = imageUrl
                });
            }

            return new UploadMultipleProductImgsResponse { Uploads = uploads };
        }
    }
}
