namespace Dotnet_Azure_Rag_Api.Models
{
    /// <summary>
    /// Represents an individual result from the Azure AI Search vector query.
    /// </summary>
    public class SearchResultItem
    {
        public string Content { get; set; } = string.Empty;
        public double Score { get; set; }
    }
}
