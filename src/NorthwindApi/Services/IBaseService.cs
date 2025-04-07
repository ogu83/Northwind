namespace NorthwindApi.Services;

public interface IBaseService<T>
{
    Task<List<T>> GetListAsync();
}
