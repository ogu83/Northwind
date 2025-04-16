using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface IProductProvider : IBaseProvider<Product, int>
{
    Task<List<Product>> GetListByCategoryIdAsync(int categoryId);
    Task<List<Product>> GetListBySupplierIdAsync(int supplierId);
}