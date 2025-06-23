using Dotnet_Azure_Rag_Api.Authorization;
using Dotnet_Azure_Rag_Api.Models;
using Dotnet_Azure_Rag_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet_Azure_Rag_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IPdfProcessor _pdfProcessor;
        private readonly IOpenAIService _openAiService;
        private readonly ISearchIndexService _indexService;
        private readonly ISearchService _searchService;
        private readonly ILogger<UploadController> _logger;

        public UploadController(IPdfProcessor pdfProcessor, IOpenAIService openAiService,
            ISearchIndexService indexService, ISearchService searchService, ILogger<UploadController> logger)
        {
            _pdfProcessor = pdfProcessor;
            _openAiService = openAiService;
            _indexService = indexService;
            _searchService = searchService;
            _logger = logger;
        }

        /// <summary>
        /// Uploads a PDF file, generates vector embeddings,and uploads the enriched documents to the Azure AI Search index.
        /// </summary>
        /// <param name="file">The PDF file to upload and process.</param>
        /// <returns>Upload PDF response.</returns>
        [HttpPost("upload-pdf")]
        [ApiKeyAuthorize] // Custom attribute to check API key
        [RequestSizeLimit(100_000_000)] // e.g., limit to ~100MB
        public async Task<IActionResult> UploadPdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("PDF file is required.");
            try
            {
                _logger.LogInformation("PDF received: {File}", file.FileName);
                byte[] pdfBytes;
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    pdfBytes = ms.ToArray();
                }
                // 1. Create or update index
                await _indexService.CreateOrUpdateIndexAsync();
                // 2. Extract chunks
                List<DocumentChunk> chunks = await _pdfProcessor.ExtractChunksAsync(pdfBytes);
                _logger.LogInformation("Extracted {Count} chunks", chunks.Count);
                // 3. For each chunk, generate parallel embedding
                var embeddingTasks = chunks.Select(async chunk =>
                {
                    chunk.Embedding = (await _openAiService.GetEmbeddingAsync(chunk.Content)).ToArray();
                });
                await Task.WhenAll(embeddingTasks);
                // 4. Upload to search index
                await _indexService.UploadDocumentsAsync(chunks);
                _logger.LogInformation("Uploaded all chunks to search index");
                return Ok(new { Count = chunks.Count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing upload");
                return StatusCode(500, new { error = "Internal server error while processing PDF." });
            }

        }
    }
}
