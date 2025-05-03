using NorthwindApi.Models;

namespace NorthwindApi.Services;

/// <summary>
/// Base Domain Service for DTOs 
/// </summary>
/// <typeparam name="T">Model class type</typeparam>
/// <typeparam name="IDT">Primary key type</typeparam>
public interface IBaseService<T, IDT>
    where T: BaseModel
{
    Task<List<T>> GetListAsync();
    Task<PagedList<T>> GetPagedListAsync(int skip, int take);
    Task<T> GetById(IDT id);
    Task<T> Update(T obj);
    Task<T> Add(T obj);
    Task Delete(IDT id);
}
