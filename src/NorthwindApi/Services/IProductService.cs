using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface IProductService : IBaseService<Product>
{
    Task<List<Product>> GetListByCategoryAsync(int categoryId);
}