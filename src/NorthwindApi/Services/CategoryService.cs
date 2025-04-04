using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class CategoryService(IMapper mapper, ICategoryProvider categoryProvider) : ICategoryService
{
    public List<Category> GetCategoryList()
    {
        var dbObj = categoryProvider.GetList();
        var retVal = mapper.Map<List<Category>>(dbObj);
        return retVal;
    }
}
