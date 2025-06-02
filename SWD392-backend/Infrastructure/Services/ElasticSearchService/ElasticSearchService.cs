using Elastic.Clients.Elasticsearch;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.ElasticSearchService
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly ElasticsearchClient _client;
        public ElasticSearchService()
        {
            var uri = Environment.GetEnvironmentVariable("ELS_URI");

            if (string.IsNullOrEmpty(uri))
                throw new Exception("env not set!");

            var settings = new ElasticsearchClientSettings(new Uri(uri));

            // Create client
            _client = new ElasticsearchClient(settings);
        }

        public async Task<ProductResponse> SearchAsync(string query)
        {
            var response = await _client.SearchAsync<ProductResponse>(s => s
                    .Indices("products")
                    .Query(q => q
                        .MultiMatch(m => m
                            .Query(query)
                            .Fields(new Field("name",))
                            )
                        )
                    );
        }
    }
}
