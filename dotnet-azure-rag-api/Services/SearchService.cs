using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Options;
using Dotnet_Azure_Rag_Api.Models;
using Dotnet_Azure_Rag_Api.Models.Options;
using Dotnet_Azure_Rag_Api.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Dotnet_Azure_Rag_Api.Services
{
    public class SearchService : ISearchService
    {
        private readonly HttpClient _httpClient;
        private readonly AzureSearchOptions _options;
        private readonly ILogger<SearchService> _logger;
        private readonly SearchClient _searchClient;

        public SearchService(IOptions<AzureSearchOptions> options, IHttpClientFactory httpFactory, ILogger<SearchService> logger)
        {
            _options = options.Value;
            _logger = logger;
            _httpClient = httpFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("api-key", _options.ApiKey);
            _searchClient = new SearchClient(new Uri(_options.Endpoint), _options.IndexName, new AzureKeyCredential(_options.ApiKey));
        }


        public async Task<IReadOnlyList<SearchResultItem>> VectorSearchAsync(IReadOnlyList<float> vector, int? k = null)
        {
            if (vector == null || vector.Count == 0)
                throw new ArgumentException("Vector cannot be null or empty", nameof(vector));

            try
            {
                int topK = k ?? _options.TopK;

                string endpoint = $"{_options.Endpoint}/indexes/{_options.IndexName}/docs/search?api-version={_options.ApiVersion}";

                var body = new
                {
                    count = true,
                    select = "Content",
                    vectorQueries = new[]
                    {
                        new
                        {
                            kind = "vector",
                            vector = vector,
                            k = topK,
                            fields = _options.VectorFieldName
                        }
                    }
                };

                var jsonBody = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");


                var response = await _httpClient.PostAsync(endpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Search request failed: {StatusCode} - {Error}", response.StatusCode, error);
                    throw new Exception($"Search request failed: {response.StatusCode} - {error}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(jsonResponse);

                var results = new List<SearchResultItem>();

                foreach (var item in doc.RootElement.GetProperty("value").EnumerateArray())
                {
                    string chunk = item.GetProperty("Content").GetString() ?? string.Empty;
                    double score = item.TryGetProperty("@search.score", out var scoreElement) ? scoreElement.GetDouble() : 0;

                    results.Add(new SearchResultItem
                    {
                        Content = chunk,
                        Score = score
                    });
                }

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Vector search via REST failed");
                throw;
            }
        }
    }
}
