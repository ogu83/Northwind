using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NorthwindApi;
using NorthwindApi.DbModels;
using NorthwindApi.Providers;
using NorthwindApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure EF Core with SQL Server
builder.Services.AddDbContext<InstnwndContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InstnwndConn")));

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
builder.Services.AddTransient<IOrderDetailsService, OrderDetailsService>();
builder.Services.AddTransient<ISupplierService, SupplierService>();

//Build the app
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c=>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind API V1");
});

app.MapControllers();
app.Run();

public partial class Program { }