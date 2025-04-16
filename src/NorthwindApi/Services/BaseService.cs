using AutoMapper;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public abstract class BaseService<T, DBT, IDT> 
    : IBaseService<T, IDT>
{
    protected readonly IBaseProvider<DBT, IDT> _provider;

    protected readonly IMapper _mapper;

    protected BaseService(IMapper mapper, IBaseProvider<DBT, IDT> provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
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
}