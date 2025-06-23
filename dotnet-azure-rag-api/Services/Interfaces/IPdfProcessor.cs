using Dotnet_Azure_Rag_Api.Models;

namespace Dotnet_Azure_Rag_Api.Services.Interfaces
{
    /// <summary>
    /// Interface for PDF Processing service.
    /// </summary>
    public interface IPdfProcessor
    {
        /// <summary>
        /// Extracts text chunks from a PDF byte array.
        /// </summary>
        /// <param name="pdfBytes">PDF file content as byte array.</param>
        /// <returns>List of DocumentChunk with Content populated.</returns>
        Task<List<DocumentChunk>> ExtractChunksAsync(byte[] pdfBytes);
    }
}
