using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Dotnet_Azure_Rag_Api.Models
{
    /// <summary>
    /// Represents a chunk of a document used in Azure Cognitive Search.
    /// </summary>
    public class DocumentChunk
    {
        [SimpleField(IsKey = true, IsFilterable = true)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [SearchableField(IsFilterable = false, IsSortable = false)]
        public string Content { get; set; } = string.Empty;

        [SimpleField(IsFilterable = false, IsSortable = false, IsFacetable = false)]
        public float[] Embedding { get; set; } = Array.Empty<float>();
    }
}
