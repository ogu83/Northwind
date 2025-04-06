using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface ICategoryProvider
{
    Task<List<Category>> GetListAsync();
    Task<Category?> GetByIdAsync(int id);
}