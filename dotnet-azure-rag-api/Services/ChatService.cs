using Dotnet_Azure_Rag_Api.Models;
using Dotnet_Azure_Rag_Api.Services.Interfaces;
using System.Text;

namespace Dotnet_Azure_Rag_Api.Services
{
    public class ChatService : IChatService
    {
        private readonly IOpenAIService _openAIService;
        private readonly ISearchService _searchService;
        private readonly ILogger<ChatService> _logger;
        private const string SYSTEM_PROMPT = "You are meant to behave as a RAG chatbot that derives its context from a database of Azure Services & IT Support stored in Azure AI Search Solution. " +
                "Answer strictly from the provided context; if the answer is not in context, politely say so. " +
                "Do not include extra information or links not in the context. " +
                "Structure your answers professionally, as if speaking like a human.";

        public ChatService(IOpenAIService openAIService, ISearchService searchService, ILogger<ChatService> logger)
        {
            _openAIService = openAIService;
            _searchService = searchService;
            _logger = logger;
        }

        private string BuildUserPrompt(string userQuery, IReadOnlyList<Models.SearchResultItem> context)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"The user query is: {userQuery}");
            sb.AppendLine("The context passages are below:");
            int i = 1;
            foreach (var item in context)
            {
                sb.AppendLine($"Passage {i++} (score {item.Score:F3}): {item.Content}");
            }
            return sb.ToString();
        }

        public async Task<ChatResponse> AnswerQueryAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new System.ArgumentException("Query cannot be empty", nameof(query));

            _logger.LogInformation("Starting RAG for query: {Query}", query);

            // 1. Embedding
            var embedding = await _openAIService.GetEmbeddingAsync(query);
            _logger.LogInformation("Embedding obtained, length={Len}", embedding.Count);

            // 2. Vector search
            var context = await _searchService.VectorSearchAsync(embedding, 3);
            _logger.LogInformation("Retrieved {Count} context passages", context.Count);

            // 3. Build prompt
            var userPrompt = BuildUserPrompt(query, context);

            // 4. Chat completion
            var answer = await _openAIService.GetChatCompletionAsync(SYSTEM_PROMPT, userPrompt);
            _logger.LogInformation("Chat completion received");

            return new ChatResponse { Answer = answer };
        }
    }
}
