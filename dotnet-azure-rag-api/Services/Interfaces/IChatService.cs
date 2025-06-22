using Dotnet_Azure_Rag_Api.Models;

namespace Dotnet_Azure_Rag_Api.Services.Interfaces
{
    /// <summary>
    /// Interface for ChatService that answers user queries using a RAG (Retrieval-Augmented Generation) approach.
    /// </summary>

    public interface IChatService
    {
        /// <summary>
        /// Processes the given user query and returns a chat response with generated answer.
        /// </summary>
        /// <param name="query">The user's input query.</param>
        /// <returns>A chat response containing the AI-generated answer.</returns>
        Task<ChatResponse> AnswerQueryAsync(string query);
    }
}
