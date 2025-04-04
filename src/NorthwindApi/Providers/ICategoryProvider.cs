using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface ICategoryProvider
{
    List<Category> GetList();
}
