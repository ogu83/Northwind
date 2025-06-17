using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Services.AddSerilog();
// builder.Logging.AddConsole(consoleLogOptions =>
// {
//     // Configure all logs to go to stderr
//     consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
// });
builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// HttpClient for MCP server tools
builder.Services.AddSingleton(_ =>
{
    var client = new HttpClient() { BaseAddress = new Uri("http://localhost:5205") };
    return client;
});

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();
    
var app = builder.Build();

app.MapMcp();
app.Run();