using SWD392_backend.Context;
using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Repositories.ReviewRepository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MyDbContext _context;

        public ReviewRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddReviewAsync(product_review review)
        {
            await _context.product_reviews.AddAsync(review);
        }

        public async Task LoadUserAsync(product_review review)
        {
            await _context.Entry(review).Reference(r => r.user).LoadAsync();
        }
    }
}
