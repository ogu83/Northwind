using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerGen();

// Add Mappers to the container
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Providers to the container
builder.Services.AddTransient<IProductProvider, ProductProvider>();
builder.Services.AddTransient<ICategoryProvider, CategoryProvider>();

// Add services to the container.
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c=>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind API V1");
});

app.MapControllers();
app.Run();

public partial class Program { }