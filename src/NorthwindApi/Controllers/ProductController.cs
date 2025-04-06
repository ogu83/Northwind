using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class ProductController(IProductService productService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Product>> GetByCategory(int categoryId)
    {
        var retVal = await productService.GetProductsByCategoryIdAsync(categoryId);
        return retVal;
    }
}