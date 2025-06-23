using Dotnet_Azure_Rag_Api.Models.Options;
using Dotnet_Azure_Rag_Api.Extensions;
using dotnet_azure_rag_api.Security;

var builder = WebApplication.CreateBuilder(args);

//Configuration Binding
builder.Services.Configure<AzureOpenAIOptions>(builder.Configuration.GetSection("AzureOpenAI"));
builder.Services.Configure<AzureSearchOptions>(builder.Configuration.GetSection("AzureSearch"));

var options = builder.Configuration.GetSection("AzureOpenAI").Get<AzureOpenAIOptions>();
Console.WriteLine($"Loaded OpenAI Endpoint: {options?.Endpoint}");
Console.WriteLine($"Loaded OpenAI Key: {(string.IsNullOrWhiteSpace(options?.ApiKey) ? "MISSING" : "LOADED")}");

//HTTP Client
builder.Services.AddHttpClient();

//Service Registrations
builder.Services.AddAppServices();

//API Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "RAG Chatbot API",
        Version = "v1"
    });

    // Define API Key scheme
    c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "API Key needed to access the Upload endpoint. Use header: `x-api-key: your_api_key`",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "x-api-key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    // Register your custom operation filter
    c.OperationFilter<ApiKeyHeaderOperationFilter>();
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();

