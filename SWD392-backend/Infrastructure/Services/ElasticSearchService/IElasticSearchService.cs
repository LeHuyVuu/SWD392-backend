using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.ElasticSearchService
{
    public interface IElasticSearchService
    {
        Task<ProductResponse> SearchAsync(string query);
    }
}
