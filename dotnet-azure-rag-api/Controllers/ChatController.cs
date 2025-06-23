using Microsoft.AspNetCore.Mvc;
using Dotnet_Azure_Rag_Api.Models;
using Dotnet_Azure_Rag_Api.Services.Interfaces;

namespace Dotnet_Azure_Rag_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;
        public ChatController(IChatService ragService, ILogger<ChatController> logger)
        {
            _chatService = ragService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a chat completion based on the provided system and user prompts.
        /// </summary>
        /// <param name="request">The request containing system and user prompts.</param>
        /// <returns>A chat completion response.</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest(new { error = "Query cannot be empty." });
            }

            try
            {
                _logger.LogInformation("Received chat request");
                ChatResponse response = await _chatService.AnswerQueryAsync(request.Question);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chat request");
                return StatusCode(500, new { error = "Internal server error while processing the request." });
            }
        }
    }
}
