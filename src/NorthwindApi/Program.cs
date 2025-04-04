using Microsoft.EntityFrameworkCore;
using NorthwindApi;
using NorthwindApi.DbModels;
using NorthwindApi.Providers;
using NorthwindApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure EF Core with SQL Server
builder.Services.AddDbContext<InstnwndContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InstnwndConn")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddTransient<ICategoryProvider, CategoryProvider>();
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