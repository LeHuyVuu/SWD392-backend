using cybersoft_final_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Services.ReviewService;
using SWD392_backend.Models;
using SWD392_backend.Models.Request;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<PagedResult<ReviewResponse>>> GetAllReviews([FromQuery] int productId, int page = 1, int pageSize = 10)
        {
            var response = await _reviewService.GetReviewsByProductIdAsync(productId, page, pageSize);
            if (response.Items == null || !response.Items.Any())
                return BadRequest(HTTPResponse<object>.Response(400, "Lấy đánh giá thất bại", response));
            else 
                return Ok(HTTPResponse<object>.Response(200, "Lấy đánh giá thành công", response));
        }

        [HttpPost("add")]
        public async Task<ActionResult<ReviewResponse>> AddReview([FromQuery] int productId, [FromBody] ReviewRequest request)
        {
            try
            {
                var role = User.FindFirst("Role")?.Value;

                if (string.IsNullOrEmpty(role))
                    return Unauthorized(HTTPResponse<object>.Response(401, "Role claim not found.", null));

                string? idClaimType = role == "CUSTOMER" ? "UserId" : role == "SUPPLIER" ? "SupplierId" : null;

                if (idClaimType == null)
                    return Unauthorized(HTTPResponse<object>.Response(401, "Unsupported role.", null));

                var idClaim = User.FindFirst(idClaimType)?.Value;

                if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out int id))
                    return BadRequest(HTTPResponse<object>.Response(400, $"Invalid or missing {idClaimType}.", null));

                var response = await _reviewService.AddReviewAsync(id, productId, request);
                if (response == null)
                    return BadRequest(HTTPResponse<object>.Response(400, "Thêm đánh giá thất bại", null));
                else
                    return Ok(HTTPResponse<ReviewResponse>.Response(200, "Thêm đánh giá thành công", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }
    }
}
