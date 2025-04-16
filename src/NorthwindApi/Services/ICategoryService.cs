using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface ICategoryService : IBaseService<Category, int>
{
    Task<byte[]?> GetPictureAsync(int id);
}
