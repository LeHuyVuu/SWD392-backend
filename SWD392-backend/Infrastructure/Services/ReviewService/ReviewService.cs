using AutoMapper;
using SWD392_backend.Entities;
using SWD392_backend.Infrastructure.Repositories.ReviewRepository;
using SWD392_backend.Models;
using SWD392_backend.Models.Request;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.ReviewService
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewResponse> AddReviewAsync(int userId, int productId, ReviewRequest request)
        {
            var review = _mapper.Map<product_review>(request);
            review.UserId = userId;
            review.ProductId = productId;
            review.CreatedAt = DateTime.UtcNow;

            // Add
            await _reviewRepository.AddReviewAsync(review);

            // Save to DB
            await _unitOfWork.SaveAsync();

            // Load user
            await _reviewRepository.LoadUserAsync(review);

            var response = _mapper.Map<ReviewResponse>(review);

            return response;
        }

        public async Task<PagedResult<ReviewResponse>> GetReviewsByProductIdAsync(int productId, int page = 1, int pageSize = 10)
        {
            var pagedResult = await _reviewRepository.GetReviewsByProductIdAsync(productId, page, pageSize);

            var response = _mapper.Map<List<ReviewResponse>>(pagedResult.Items);

            return new PagedResult<ReviewResponse>
            {
                Items = response,
                TotalItems = pagedResult.TotalItems,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize
            };
        }
    }
}
