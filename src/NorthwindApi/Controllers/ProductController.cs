using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class ProductController(IProductService productService)
    : EntityApiControllerBase<Product, IProductService, int>(productService)
{
    [HttpGet("Category/{id}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetByCategory(int id)
    {
        var retVal = await productService.GetListByCategoryAsync(id);
        return retVal;
    }

    [HttpGet("Supplier/{id}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetBySupplier(int id)
    {
        var retVal = await productService.GetListBySupplier(id);
        return retVal;
    }
}
