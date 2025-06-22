using Dotnet_Azure_Rag_Api.Models;

namespace Dotnet_Azure_Rag_Api.Services.Interfaces
{
    /// <summary>
    /// Interface for AI Search service.
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Performs a vector similarity search against the Azure AI Search index.
        /// </summary>
        /// <param name="vector">The input vector to compare against indexed embeddings.</param>
        /// <param name="k">Optional. Number of top matching results to retrieve.</param>
        /// <returns>List of search result items ranked by similarity score.</returns>
        Task<IReadOnlyList<SearchResultItem>> VectorSearchAsync(IReadOnlyList<float> vector, int? k);
    }
}
