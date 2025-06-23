using Dotnet_Azure_Rag_Api.Models;

namespace Dotnet_Azure_Rag_Api.Services.Interfaces
{
    /// <summary>
    /// Interface for AI Search Index service.
    /// </summary>
    public interface ISearchIndexService
    {
        /// <summary>
        /// Creates or updates the Azure Search index with a vector field.
        /// </summary>
        Task CreateOrUpdateIndexAsync();

        /// <summary>
        /// Uploads chunks with embeddings to the Azure Search index.
        /// </summary>
        /// <param name="chunks">List of DocumentChunk with Content populated.</param>
        Task UploadDocumentsAsync(IEnumerable<DocumentChunk> chunks);
    }
}
