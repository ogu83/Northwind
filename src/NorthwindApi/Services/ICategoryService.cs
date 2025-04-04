using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface ICategoryService
{
    List<Category> GetCategoryList();
}
