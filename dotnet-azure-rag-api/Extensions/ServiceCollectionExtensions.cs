using Dotnet_Azure_Rag_Api.Services.Interfaces;
using Dotnet_Azure_Rag_Api.Services;

namespace Dotnet_Azure_Rag_Api.Extensions
{
    /// <summary>
    /// Extension method to register application services into the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IOpenAIService, OpenAIService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IChatService, ChatService>();
            return services;
        }
    }
}
