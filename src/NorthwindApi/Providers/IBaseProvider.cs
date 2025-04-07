namespace NorthwindApi.Providers;

public interface IBaseProvider<T>
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetListAsync();
}
