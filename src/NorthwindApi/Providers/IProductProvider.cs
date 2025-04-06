using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface IProductProvider
{
    Task<List<Product>> GetListAsync();

    Task<List<Product>> GetListByCategoryIdAsync(int categoryId);
}
