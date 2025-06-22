using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;         // for ChatClient, ChatMessage, ChatCompletion, ChatMessageContentPart
using OpenAI.Embeddings;   // for EmbeddingClient, OpenAIEmbedding
using Dotnet_Azure_Rag_Api.Models.Options;
using Dotnet_Azure_Rag_Api.Services.Interfaces;

namespace Dotnet_Azure_Rag_Api.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly AzureOpenAIClient _azureClient;
        private readonly string _embeddingDeployment;
        private readonly string _chatDeployment;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIService(
            IOptions<AzureOpenAIOptions> options,
            ILogger<OpenAIService> logger)
        {
            var opt = options.Value;
            _logger = logger;
            _azureClient = new AzureOpenAIClient(
                new Uri(opt.Endpoint),
                new AzureKeyCredential(opt.ApiKey)
            );
            _embeddingDeployment = opt.EmbeddingDeploymentName;
            _chatDeployment = opt.ChatDeploymentName;
        }

        public async Task<IReadOnlyList<float>> GetEmbeddingAsync(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be empty.", nameof(input));

            try
            {
                // Get the EmbeddingClient for the specified deployment
                EmbeddingClient embeddingClient = _azureClient.GetEmbeddingClient(_embeddingDeployment);
                OpenAIEmbedding embedResult = await embeddingClient.GenerateEmbeddingAsync(input);
                // Convert to float[]
                return embedResult.ToFloats().ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get embeddings: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<string> GetChatCompletionAsync(string systemPrompt, string userPrompt)
        {
            try
            {
                // Get the ChatClient for the specified deployment
                ChatClient chatClient = _azureClient.GetChatClient(_chatDeployment);

                // Build messages: parameterless ChatMessage, set Role and Content
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(userPrompt)
                };

                ChatCompletion completion = await chatClient.CompleteChatAsync(messages);

                // Extract content: completion.Content is IReadOnlyList<ChatMessageContentPart>
                if (completion.Content is IReadOnlyList<ChatMessageContentPart> parts && parts.Count > 0)
                {
                    // Concatenate all text parts
                    return string.Concat(parts.Select(p => p.Text));
                }
                _logger.LogWarning("Chat completion returned no content.");
                return "Sorry, I couldn't find a suitable answer.";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get chat completion: {Message}", ex.Message);
                throw;
            }
        }
    }
}
