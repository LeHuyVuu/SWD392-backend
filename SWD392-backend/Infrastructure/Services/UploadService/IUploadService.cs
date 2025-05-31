using SWD392_backend.Models.Request;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.UploadService
{
    public interface IUploadService
    {
        Task<UploadMainProductImgResponse> UploadImage(UploadMainProductImgRequest request);
    }
}
