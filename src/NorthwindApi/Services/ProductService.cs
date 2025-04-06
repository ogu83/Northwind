using System.Threading.Tasks;
using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class ProductService(IMapper mapper, IProductProvider productProvider) : IProductService
{
    public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var dbObj = await productProvider.GetListByCategoryIdAsync(categoryId);
        var retVal = mapper.Map<List<Product>>(dbObj);
        return retVal;
    }
}