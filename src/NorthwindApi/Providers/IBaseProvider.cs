namespace NorthwindApi.Providers;

/// <summary>
/// Base Provider Interface for the Data Base Providers
/// </summary>
/// <typeparam name="T">Entity Type</typeparam>
/// <typeparam name="IDT">Primary Key Type</typeparam>
public interface IBaseProvider<T, IDT>
{
    Task<T?> GetByIdAsync(IDT id);
    Task<List<T>> GetListAsync();
    Task<T> AddAsync(T entity);
    Task<T> AddAndSaveAsync(T entity);
    T Update(T entity);
    Task<T> UpdateAndSaveAsync(T entity);
}