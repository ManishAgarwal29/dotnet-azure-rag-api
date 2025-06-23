using Dotnet_Azure_Rag_Api.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace dotnet_azure_rag_api.Security
{
    public class ApiKeyHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasApiKeyAuth = context.MethodInfo.GetCustomAttribute<ApiKeyAuthorizeAttribute>() != null;

            if (!hasApiKeyAuth)
                return;

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        Array.Empty<string>()
                    }
                }

            };
        }
    }
}
