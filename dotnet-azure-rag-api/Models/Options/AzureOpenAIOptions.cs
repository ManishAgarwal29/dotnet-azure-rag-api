using System.ComponentModel.DataAnnotations;

namespace Dotnet_Azure_Rag_Api.Models.Options
{
    /// <summary>
    /// Configuration options for Azure OpenAI integration.
    /// </summary>
    public class AzureOpenAIOptions
    {
        [Required]
        public string Endpoint { get; set; } = string.Empty;
        [Required]
        public string ApiKey { get; set; } = string.Empty;
        [Required]
        public string EmbeddingDeploymentName { get; set; } = string.Empty;
        [Required]
        public string ChatDeploymentName { get; set; } = string.Empty;
        [Required]
        public string ApiVersion { get; set; } = string.Empty;
    }
}
