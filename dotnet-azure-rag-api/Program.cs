using Dotnet_Azure_Rag_Api.Models.Options;
using Dotnet_Azure_Rag_Api.Extensions;

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
builder.Services.AddSwaggerGen();

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

