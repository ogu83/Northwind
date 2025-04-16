namespace NorthwindApi.Providers;

public interface IBaseProvider<T, IDT>
{
    Task<T?> GetByIdAsync(IDT id);
    Task<List<T>> GetListAsync();
}