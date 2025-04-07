using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class CategoryService(IMapper mapper, ICategoryProvider categoryProvider) 
    : BaseService<Category, DbModels.Category>(mapper,categoryProvider), ICategoryService
{
    public async Task<byte[]?> GetPictureAsync(int id)
    {
        var dbObj = await categoryProvider.GetByIdAsync(id);
        return dbObj?.Picture;
    }
}
