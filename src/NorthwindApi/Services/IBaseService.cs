namespace NorthwindApi.Services;

public interface IBaseService<T, IDT>
{
    Task<List<T>> GetListAsync();
    Task<T> GetById(IDT id);
}
