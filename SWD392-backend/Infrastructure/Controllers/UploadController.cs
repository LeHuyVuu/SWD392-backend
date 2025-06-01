using System.Threading.Tasks;
using cybersoft_final_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Infrastructure.Services.S3Service;
using SWD392_backend.Infrastructure.Services.UploadService;
using SWD392_backend.Models.Request;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly IUploadService _uploadService;

        public UploadController(IS3Service s3Service, IUploadService uploadService)
        {
            _s3Service = s3Service;
            _uploadService = uploadService;
        }

        [HttpPost("upload-images")]
        public async Task<ActionResult<UploadProductImgResponse>> GetPresignedUrl([FromBody] UploadProductImgsRequest request)
        {

            var upload = await _uploadService.UploadMultipleImage(request);

            if (upload == null)
                return BadRequest(HTTPResponse<object>.Response(400, "Có lỗi xảy ra", null));

            return Ok(HTTPResponse<object>.Response(200, "Successfully", upload));
        }
    }
}
