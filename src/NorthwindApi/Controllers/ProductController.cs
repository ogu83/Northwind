using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class ProductController(IProductService productService)
    : EntityApiControllerBase<Product, IProductService, int>(productService)
{
    /// <summary>
    /// Returns Products in a Category
    /// </summary>
    /// <param name="id">Category Id</param>
    /// <returns>Products in a category</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("Category/{id}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetByCategory(int id)
    {
        var retVal = await productService.GetListByCategoryAsync(id);
        return retVal;
    }

    /// <summary>
    /// Returns Products of a Supplier
    /// </summary>
    /// <param name="id">Supplier Id</param>
    /// <returns>Products of a Supplier</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("Supplier/{id}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetBySupplier(int id)
    {
        var retVal = await productService.GetListBySupplier(id);
        return retVal;
    }
}
