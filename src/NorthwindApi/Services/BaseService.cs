using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public abstract class BaseService<T, DBT, IDT>(IMapper mapper, IBaseProvider<DBT, IDT> provider)
    : IBaseService<T, IDT>
    where T : BaseModel
    where DBT : class
{
    protected readonly IBaseProvider<DBT, IDT> _provider = provider ?? throw new ArgumentNullException(nameof(provider));

    protected readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<List<T>> GetListAsync()
    {
        var dbObj = await _provider.GetListAsync();
        var retVal = _mapper.Map<List<T>>(dbObj);
        return retVal;
    }

    public async Task<T> GetById(IDT id)
    {
        var dbObj = await _provider.GetByIdAsync(id);
        var retVal = _mapper.Map<T>(dbObj);
        return retVal;
    }

    public async Task<T> Update(T obj)
    {
        var dbObj = _mapper.Map<DBT>(obj);
        dbObj = await _provider.UpdateAndSaveAsync(dbObj);
        var retVal = _mapper.Map<T>(dbObj);
        return retVal;
    }

    public async Task<T> Add(T obj)
    {
        var dbObj = _mapper.Map<DBT>(obj);
        dbObj = await _provider.AddAndSaveAsync(dbObj);
        var retVal = _mapper.Map<T>(dbObj);
        return retVal;
    }

    public async Task Delete(IDT id)
    {
        await _provider.DeleteAndSaveAsync(id);
    }
}