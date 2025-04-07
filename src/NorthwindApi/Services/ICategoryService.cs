using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface ICategoryService : IBaseService<Category>
{
    Task<byte[]?> GetPictureAsync(int id);
}
