using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NorthwindApi;
using NorthwindApi.DbModels;
using NorthwindApi.Providers;
using NorthwindApi.Services;
using Serilog;
using Serilog.Events;

// Log.Logger = new LoggerConfiguration()
//     .WriteTo.Console()
//     .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole(); 

// Configure EF Core with SQL Server
builder.Services.AddDbContext<InstnwndContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InstnwndConn")));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJsFrontend",
        policy => policy.WithOrigins("http://localhost:5206", "https://localhost:7252")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
    options.AddPolicy("AllowAngularFrontend",
        policy => policy.WithOrigins("http://localhost:5207", "https://localhost:7253")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Add Controllers to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger to the container
builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "This is Northwind API",
        Version = "v1",
        Description = "This an .NET 9.0 WEB API for Northwind Database"
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

});

// Add Mappers to the container
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Providers to the container
builder.Services.AddTransient<IProductProvider, ProductProvider>();
builder.Services.AddTransient<ICategoryProvider, CategoryProvider>();
builder.Services.AddTransient<ICustomerProvider, CustomerProvider>();
builder.Services.AddTransient<IOrderProvider, OrderProvider>();
builder.Services.AddTransient<IOrderDetailsProvider, OrderDetailsProvider>();
builder.Services.AddTransient<ISupplierProvider, SupplierProvider>();

// Add services to the container.
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IOrderDetailService, OrderDetailService>();
builder.Services.AddTransient<ISupplierService, SupplierService>();

//Response Cache
// builder.Services.AddResponseCaching();

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
app.UseCors("AllowAngularFrontend");

//SWAGGER
app.UseSwagger();
app.UseSwaggerUI(c=>
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