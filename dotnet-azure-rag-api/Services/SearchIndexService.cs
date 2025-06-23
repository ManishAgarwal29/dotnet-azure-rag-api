using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using Dotnet_Azure_Rag_Api.Models;
using Dotnet_Azure_Rag_Api.Models.Options;
using Dotnet_Azure_Rag_Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Azure;

namespace Dotnet_Azure_Rag_Api.Services
{
    public class SearchIndexService : ISearchIndexService
    {
        //private readonly SearchIndexClient _indexClient;
        private readonly HttpClient _httpClient;
        private readonly AzureSearchOptions _options;
        private readonly ILogger<SearchIndexService> _logger;
        private readonly SearchClient _searchClient;

        public SearchIndexService(IOptions<AzureSearchOptions> options, ILogger<SearchIndexService> logger, IHttpClientFactory httpFactory)
        {
            _options = options.Value;
            _logger = logger;
            _httpClient = httpFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("api-key", _options.ApiKey);
            _searchClient = new SearchClient(new Uri(_options.Endpoint), _options.IndexName, new AzureKeyCredential(_options.ApiKey));
        }


        public async Task CreateOrUpdateIndexAsync()
        {
            string indexName = _options.IndexName;
            string? endpoint = _options.Endpoint?.TrimEnd('/');
            string getUrl = $"{endpoint}/indexes/{indexName}?api-version={_options.IndexApiVersion}";
            // Check if index exists
            var getResponse = await _httpClient.GetAsync(getUrl);
            if (getResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation("Index {IndexName} already exists.", indexName);
                return;
            }
            // Define index JSON with vector field searchable:true
            var indexJson = new
            {
                name = indexName,
                fields = new object[]
                {
                    new { name = "Id", type = "Edm.String", key = true, filterable = true },
                    new { name = "Content", type = "Edm.String", searchable = true },
                    new {
                        name = _options.VectorFieldName,
                        type = "Collection(Edm.Single)",
                        // For vector field, searchable must be true
                        searchable = true,
                        filterable = false,
                        sortable = false,
                        facetable = false,
                        dimensions = _options.VectorDimensions,
                        vectorSearchProfile = _options.VectorSearchProfileName
                    }
                },
                vectorSearch = new
                {
                    profiles = new[] {
                        new {
                            name = _options.VectorSearchProfileName,
                            algorithm = "hnsw"
                        }
                    },
                    algorithms = new[] {
                        new { name = "hnsw", kind = "hnsw" }
                    }
                }
            };
            string putUrl = $"{endpoint}/indexes/{indexName}?api-version={_options.IndexApiVersion}";
            string json = JsonSerializer.Serialize(indexJson);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var putResponse = await _httpClient.PutAsync(putUrl, content);
            if (!putResponse.IsSuccessStatusCode)
            {
                string err = await putResponse.Content.ReadAsStringAsync();
                _logger.LogError("Failed to create index {IndexName}: {Error}", indexName, err);
                throw new Exception($"Index creation failed: {err}");
            }
            _logger.LogInformation("Index {IndexName} created successfully.", indexName);
        }

        public async Task UploadDocumentsAsync(IEnumerable<DocumentChunk> chunks)
        {
            var actions = chunks.Select(c => IndexDocumentsAction.Upload(c)).ToArray();
            var batch = IndexDocumentsBatch.Create(actions);
            try
            {
                await _searchClient.IndexDocumentsAsync(batch);
                _logger.LogInformation("Uploaded {Count} documents.", actions.Length);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload documents to search index.");
                throw;
            }
        }

    }

}
    
