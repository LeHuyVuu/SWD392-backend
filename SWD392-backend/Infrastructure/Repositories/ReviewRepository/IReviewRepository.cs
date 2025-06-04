using SWD392_backend.Entities;

namespace SWD392_backend.Infrastructure.Repositories.ReviewRepository
{
    public interface IReviewRepository
    {
        Task AddReviewAsync(product_review review);
        Task LoadUserAsync(product_review review);
    }
}
