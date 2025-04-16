using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class ProductService(IMapper mapper, IProductProvider productProvider)
    : BaseService<Product, DbModels.Product>(mapper, productProvider), IProductService
{
    public async Task<List<Product>> GetListByCategoryAsync(int categoryId)
    {
        var dbObj = await productProvider.GetListByCategoryIdAsync(categoryId);
        var retVal = _mapper.Map<List<Product>>(dbObj);
        return retVal;
    }

    public async Task<List<Product>> GetListBySupplier(int supplierId)
    {
        var dbObj = await productProvider.GetListBySupplierIdAsync(supplierId);
        var retVal = _mapper.Map<List<Product>>(dbObj);
        return retVal;
    }
}
