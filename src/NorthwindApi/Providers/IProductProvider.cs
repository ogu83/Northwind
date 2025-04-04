using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface IProductProvider
{
    List<Product> GetList();

    List<Product> GetListByCategoryId(int categoryId);
}
