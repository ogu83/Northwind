using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface IProductService
{
    Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
}
