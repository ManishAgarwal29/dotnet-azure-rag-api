using System.ComponentModel.DataAnnotations;

namespace Dotnet_Azure_Rag_Api.Models
{
    /// <summary>
    /// Represents the request payload for the chat API.
    /// </summary>
    public class ChatRequest
    {
        [Required(ErrorMessage = "Question is required")]
        [MinLength(3, ErrorMessage = "Question must be at least 3 characters")]
        public string Question { get; set; } = string.Empty;
    }

}