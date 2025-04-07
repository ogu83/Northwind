using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface IProductProvider : IBaseProvider<Product>
{
    Task<List<Product>> GetListByCategoryIdAsync(int categoryId);
}