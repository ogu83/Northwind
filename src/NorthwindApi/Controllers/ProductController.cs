using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class ProductController(IProductService productService) : ApiControllerBase
{
    [HttpGet]
    public IEnumerable<Product> GetByCategory(int categoryId)
    {
        var retVal = productService.GetProductsByCategoryId(categoryId);
        return retVal;
    }
}