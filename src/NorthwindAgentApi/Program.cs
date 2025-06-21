using System.Reflection;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;
using OpenAI;
using System.ClientModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using NorthwindAgentApi;

var builder = WebApplication.CreateBuilder(args);

JsonSerializerOptions JsonSerializerOptions = new()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

// Logging
builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJsFrontend",
        policy => policy.WithOrigins("http://localhost:5206", "https://localhost:7252")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Add Controllers to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger to the container
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "This is Northwind MCP Agent API",
        Version = "v1",
        Description = "This an .NET 9.0 Northwind MCP Agent API"
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

});


//Read MCP info from JSON file
if (!File.Exists("mcp.info.json"))
    throw new FileNotFoundException("MCP info file not found. Please ensure 'mcp.info.json' exists in the application root directory.");

var mcpInfo = JsonSerializer.Deserialize<McpInfo>(File.ReadAllText("mcp.info.json"), JsonSerializerOptions);

if (mcpInfo == null)
{
    throw new InvalidOperationException("Failed to deserialize MCP info. Please check the 'mcp.info.json' file format.");
}

if (string.IsNullOrEmpty(mcpInfo.ApiKey))
{
    Console.WriteLine("Please set your OpenAI API key in the apiKey variable.");
    return;
}

if (string.IsNullOrEmpty(mcpInfo.Url))
{
    Console.WriteLine("Please set the URL of the MCP server in the Url variable.");
    return;
}

if (string.IsNullOrEmpty(mcpInfo.ModelId))
{
    Console.WriteLine("Please set the Model ID in the ModelId variable.");
    return;
}

var apiKeyCredential = new ApiKeyCredential(mcpInfo.ApiKey);
var openAIClient = new OpenAIClient(apiKeyCredential);

var chatClient = openAIClient
    .GetChatClient(mcpInfo.ModelId)
    .AsIChatClient()
    .AsBuilder()
    .UseFunctionInvocation()
    .Build();

builder.Services.AddSingleton(chatClient);

var transport = new SseClientTransport(new() { Endpoint = new Uri(mcpInfo.Url) });
IMcpClient mcpClient = await McpClientFactory.CreateAsync(transport);
builder.Services.AddSingleton(mcpClient);

var chatSessions = new Dictionary<Guid, ChatSession>();
builder.Services.AddSingleton(chatSessions);

//Build the app
var app = builder.Build();

//SeriLog request Logging
app.UseSerilogRequestLogging(options =>
{
    // Customize the message template
    options.MessageTemplate = "Handled {RequestPath}";

    // Emit debug-level events instead of the defaults
    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

    // Attach additional properties to the request completion event
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value ?? "");
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    };
});

//Error Logging
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//CORS
app.UseCors("AllowNextJsFrontend");

//SWAGGER
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind API V1");
    c.InjectStylesheet("https://cdn.jsdelivr.net/npm/swagger-ui-themes@3.0.1/themes/3.x/theme-newspaper.min.css");
});

//CONTROLLERS
app.MapControllers();

//RESPONSE CACHE
app.UseResponseCaching();

app.Run();

public partial class Program { }

public record McpInfo(string ApiKey, string Url, string ModelId);

public record McpMethod(string Name)
{
    public List<McpParameter> Parameters { get; set; } = [];
}

public record McpParameter(string Name, string Type);
