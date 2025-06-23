using Dotnet_Azure_Rag_Api.Models;
using Dotnet_Azure_Rag_Api.Services.Interfaces;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Dotnet_Azure_Rag_Api.Services
{
    public class PdfProcessor : IPdfProcessor
    {
        private readonly int _chunkSize;
        private readonly int _chunkOverlap;

        public PdfProcessor(int chunkSize = 1000, int chunkOverlap = 200)
        {
            _chunkSize = chunkSize;
            _chunkOverlap = chunkOverlap;
        }

        public Task<List<DocumentChunk>> ExtractChunksAsync(byte[] pdfBytes)
        {
            var chunks = new List<DocumentChunk>();
            using var stream = new MemoryStream(pdfBytes);
            using var document = PdfDocument.Open(stream);
            var sb = new StringBuilder();
            foreach (Page page in document.GetPages())
            {
                string text = page.Text;
                sb.AppendLine(text);
            }
            string allText = sb.ToString().Replace("\r", "");
            int start = 0;
            while (start < allText.Length)
            {
                int length = Math.Min(_chunkSize, allText.Length - start);
                string chunkText = allText.Substring(start, length);
                chunks.Add(new DocumentChunk { Content = chunkText });
                start += _chunkSize - _chunkOverlap;
            }
            return Task.FromResult(chunks);
        }
    }
}
