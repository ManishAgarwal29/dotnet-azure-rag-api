using System.ComponentModel.DataAnnotations;

namespace Dotnet_Azure_Rag_Api.Models.Options
{
    /// <summary>
    /// Azure Cognitive Search configuration options.
    /// </summary>
    public class AzureSearchOptions
    {
        [Required]
        public string Endpoint { get; set; } = string.Empty;
        [Required]
        public string IndexName { get; set; } = string.Empty;
        [Required]
        public string ApiKey { get; set; } = string.Empty;
        [Required]
        public string VectorFieldName { get; set; } = string.Empty;
        public int TopK { get; set; } = 3;
        public string ApiVersion { get; set; } = "2023-11-01";
        public int VectorDimensions { get; set; } = 1536;
        public string IndexApiVersion { get; set; } = "2023-11-01";
        public string VectorSearchProfileName { get; set; } = "default-vector-profile";

    }
}
