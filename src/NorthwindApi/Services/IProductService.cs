using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface IProductService
{
    List<Product> GetProductsByCategoryId(int categoryId);
}
