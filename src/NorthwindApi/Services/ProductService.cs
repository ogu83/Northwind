using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class ProductService(IMapper mapper, IProductProvider productProvider) : IProductService
{
    public List<Product> GetProductsByCategoryId(int categoryId)
    {
        var dbObj = productProvider.GetListByCategoryId(categoryId);
        var retVal = mapper.Map<List<Product>>(dbObj);
        return retVal;
    }
}