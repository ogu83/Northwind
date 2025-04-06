using System.Collections.ObjectModel;
using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class CategoryService(IMapper mapper, ICategoryProvider categoryProvider) : ICategoryService
{
    public async Task<List<Category>> GetCategoryListAsync()
    {
        var dbObj = await categoryProvider.GetListAsync();
        var retVal = mapper.Map<List<Category>>(dbObj);
        return retVal;
    }

    public async Task<byte[]?> GetPictureAsync(int id)
    {
        var dbObj = await categoryProvider.GetByIdAsync(id);
        return dbObj?.Picture;
    }
}
