using System.Security.Cryptography;
using AutoMapper;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public abstract class BaseService<T, DBT>()
{
    protected readonly IBaseProvider<DBT> _provider;
    protected readonly IMapper _mapper;

    protected BaseService(IMapper mapper, IBaseProvider<DBT> provider)
        :this()
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
}
