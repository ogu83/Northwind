using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface IProductService : IBaseService<Product, int>
{
    Task<List<Product>> GetListByCategoryAsync(int categoryId);
    Task<List<Product>> GetListBySupplier(int supplierId);
}
