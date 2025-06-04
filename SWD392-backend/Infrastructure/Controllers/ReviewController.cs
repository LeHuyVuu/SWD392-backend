using cybersoft_final_project.Models;
using Elastic.Clients.Elasticsearch.Requests;
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

        /// <summary>
        /// Lấy danh sách đánh giá theo ID sản phẩm với phân trang.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần lấy đánh giá.</param>
        /// <param name="page">Số trang, mặc định là 1.</param>
        /// <param name="pageSize">Số lượng đánh giá trên mỗi trang, mặc định là 10.</param>
        /// <returns>Trả về danh sách đánh giá phân trang hoặc thông báo lỗi nếu thất bại.</returns>
        /// <response code="200">Lấy danh sách đánh giá thành công.</response>
        /// <response code="400">Lấy danh sách đánh giá thất bại do không tìm thấy đánh giá.</response>
        [HttpGet("all")]
        public async Task<ActionResult<PagedResult<ReviewResponse>>> GetAllReviews([FromQuery] int productId, int page = 1, int pageSize = 10)
        {
            var response = await _reviewService.GetReviewsByProductIdAsync(productId, page, pageSize);
            if (response.Items == null || !response.Items.Any())
                return BadRequest(HTTPResponse<object>.Response(400, "Lấy đánh giá thất bại", response));
            else 
                return Ok(HTTPResponse<object>.Response(200, "Lấy đánh giá thành công", response));
        }

        /// <summary>
        /// Thêm đánh giá mới cho một sản phẩm.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần thêm đánh giá.</param>
        /// <param name="request">Thông tin đánh giá bao gồm nội dung và điểm số.</param>
        /// <returns>Trả về thông tin đánh giá vừa thêm hoặc thông báo lỗi nếu thất bại.</returns>
        /// <response code="200">Thêm đánh giá thành công.</response>
        /// <response code="400">Thêm đánh giá thất bại do dữ liệu không hợp lệ hoặc do đã tồn tại đánh giá.</response>
        /// <response code="500">Lỗi máy chủ nội bộ.</response>
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

        /// <summary>
        /// Cập nhật đánh giá hiện có của một sản phẩm.
        /// </summary>
        /// <param name="productId">ID của sản phẩm có đánh giá cần cập nhật.</param>
        /// <param name="request">Thông tin cập nhật của đánh giá bao gồm nội dung và điểm số.</param>
        /// <returns>Trả về thông tin đánh giá đã cập nhật hoặc thông báo lỗi nếu thất bại.</returns>
        /// <response code="200">Cập nhật đánh giá thành công.</response>
        /// <response code="400">Cập nhật đánh giá thất bại do dữ liệu không hợp lệ.</response>
        /// <response code="401">Không được phép do thiếu hoặc vai trò không hợp lệ.</response>
        /// <response code="500">Lỗi máy chủ nội bộ.</response>
        [HttpPut("update")]
        public async Task<ActionResult<ReviewResponse>> UpdateReview([FromQuery] int productId, [FromBody] ReviewRequest request)
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

                var response = await _reviewService.UpdateReviewAsync(id, productId, request);
                if (response == null)
                    return BadRequest(HTTPResponse<object>.Response(400, "Cập nhật đánh giá thất bại", null));
                else
                    return Ok(HTTPResponse<ReviewResponse>.Response(200, "Cập nhật đánh giá thành công", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }

        /// <summary>
        /// Xóa đánh giá của một sản phẩm.
        /// </summary>
        /// <param name="productId">ID của sản phẩm có đánh giá cần xóa.</param>
        /// <returns>Trả về thông báo xóa thành công hoặc thông báo lỗi nếu thất bại.</returns>
        /// <response code="200">Xóa đánh giá thành công.</response>
        /// <response code="400">Xóa đánh giá thất bại do dữ liệu không hợp lệ.</response>
        /// <response code="401">Không được phép do thiếu hoặc vai trò không hợp lệ.</response>
        /// <response code="500">Lỗi máy chủ nội bộ.</response>
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveReview([FromQuery] int productId)
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

                var response = await _reviewService.RemoveReview(id, productId);
                if (response == false)
                    return BadRequest(HTTPResponse<object>.Response(400, "Xóa đánh giá thất bại", false));
                else
                    return Ok(HTTPResponse<object>.Response(200, "Xóa đánh giá thành công", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, HTTPResponse<object>.Response(500, "Internal server error", ex.Message));
            }
        }
    }
}
