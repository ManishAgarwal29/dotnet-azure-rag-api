namespace Dotnet_Azure_Rag_Api.Services.Interfaces
{
    /// <summary>
    /// Interface for OpenAI service.
    /// </summary>
    public interface IOpenAIService
    {
        /// <summary>
        /// Generates an embedding for the given input text.
        /// </summary>
        /// <param name="input">The input string to generate embeddings for.</param>
        /// <returns>A list of floating point numbers representing the text embedding.</returns>
        Task<IReadOnlyList<float>> GetEmbeddingAsync(string input);

        /// <summary>
        /// Gets a chat-based completion using system and user prompts.
        /// </summary>
        /// <param name="systemPrompt">The system prompt representing background context or role.</param>
        /// <param name="userPrompt">The user question or message to respond to.</param>
        /// <returns>The AI-generated response as a string.</returns>
        Task<string> GetChatCompletionAsync(string systemPrompt, string userPrompt);
    }
}