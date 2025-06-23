using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dotnet_Azure_Rag_Api.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private const string API_KEY_HEADER = "x-api-key";
        private const string ENV_VAR_NAME = "UPLOAD_API_KEY";

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var expectedApiKey = config[ENV_VAR_NAME];

            if (!context.HttpContext.Request.Headers.TryGetValue(API_KEY_HEADER, out var providedKey) ||
                string.IsNullOrEmpty(expectedApiKey) ||
                providedKey != expectedApiKey)
            {
                context.Result = new UnauthorizedObjectResult(new { error = "Unauthorized. Invalid or missing API key." });
            }

            await Task.CompletedTask;
        }
    }
}
