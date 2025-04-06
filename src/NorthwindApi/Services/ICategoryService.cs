using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface ICategoryService
{
    Task<List<Category>> GetCategoryListAsync();

    Task<byte[]?> GetPictureAsync(int id);
}
