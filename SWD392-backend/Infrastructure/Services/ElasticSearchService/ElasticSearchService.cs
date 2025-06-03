using System.Linq;
using AutoMapper;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.TermVectors;
using Elastic.Clients.Elasticsearch.QueryDsl;
using SWD392_backend.Entities;
using SWD392_backend.Models;
using SWD392_backend.Models.ElasticDocs;
using SWD392_backend.Models.Response;

namespace SWD392_backend.Infrastructure.Services.ElasticSearchService
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly ElasticsearchClient _client;
        private readonly IMapper _mapper;
        public ElasticSearchService(IMapper mapper)
        {
            var uri = Environment.GetEnvironmentVariable("ELS_URI");

            if (string.IsNullOrEmpty(uri))
                throw new Exception("env not set!");

            var settings = new ElasticsearchClientSettings(new Uri(uri));

            // Create client
            _client = new ElasticsearchClient(settings);
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductResponse>> SearchAsync(
            string q = "",
            int? categoryId = null,
            int page = 1,
            int size = 10,
            string sortBy = "createdAt",
            string sortOrder = "desc"
        )
        {
            List<ProductResponse> response;

            var order = sortOrder.ToLower() == "asc" ? SortOrder.Asc : SortOrder.Desc;

            // Filter category
            var filters = new List<Query>();

            if (categoryId != null)
                filters.Add(new TermQuery("categoriesId")
                {
                    Value = categoryId.Value
                });

            // Create query
            Query query;
            if (string.IsNullOrEmpty(q))
                query = new BoolQuery
                {
                    Filter = filters
                };
            else
            {
                var shouldQuery = new List<Query>
                {
                    new MultiMatchQuery
                    {
                        Query = q,
                        Fields = new[] {
                            "name.vi^3",
                            "slug^2",
                            "description.vi"
                        },
                        Operator = Operator.And,
                        MinimumShouldMatch = "75%"
                    },
                    new MatchPhrasePrefixQuery("name.autocomplete")
                    {
                        Query = q,
                        Boost = 4
                    }
                };

                query = new BoolQuery
                {
                    Should = shouldQuery,
                    Filter = filters,
                    MinimumShouldMatch = 1
                };
            }

            var searchResponse = await _client.SearchAsync<ProductResponse>(s => s
                    .Indices("products")
                    .Sort(sort => sort.Field(sortBy, new FieldSort { Order = order }))
                    .From((page - 1) * size)
                    .Size(size)
                    .Query(query)
                );

            response = searchResponse.Hits.Select(h => h.Source).ToList();
            return new PagedResult<ProductResponse>
            {
                Items = response,
                TotalItems = (int)searchResponse.Total,
                Page = page,
                PageSize = size,               
            };

        }

        public async Task IndexProductAsync(product product)
        {
            var doc = _mapper.Map<ProductElasticDoc>(product);
            var id = product.Id.ToString();

            await _client.IndexAsync(doc, x => x.Index("products").Id(id));
        }

        public Task UpdateProductAsync(product product)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStatusProductAsync(product product)
        {
            throw new NotImplementedException();
        }
    }
}
