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
    Task<List<T>> GetListAsync(int skip, int take);
    Task<List<T>> GetListAsync(int skip, int take, string orderBy, bool isAscending);
    Task<int> GetTotalCount();
    Task<T> AddAsync(T entity);
    Task<T> AddAndSaveAsync(T entity);
    T Update(T entity);
    Task<T> UpdateAndSaveAsync(T entity);
    Task Delete(IDT id);
    Task DeleteAndSaveAsync(IDT id);
}