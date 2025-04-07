using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class ProductController(IProductService productService)
    : EntityApiControllerBase<Product, IProductService>(productService)
{
    [HttpGet("ByCategory/{id}")]
    public async Task<IEnumerable<Product>> GetByCategory(int id)
    {
        var retVal = await productService.GetListByCategoryAsync(id);
        return retVal;
    }
}
