using System.Buffers.Text;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class CategoryController(ICategoryService categoryService) : ApiControllerBase
{
    /// <summary>
    /// Get Categories List
    /// </summary>
    /// <returns>All categories</returns>
    /// <response code="200">Returns all categories</response>
    /// <response code="404">If there is no categories</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Category>>> Get()
    {
        var retVal = await categoryService.GetCategoryListAsync();
        if (retVal.Count > 0)
            return new ActionResult<IEnumerable<Category>>(retVal);
        else
            return new NotFoundResult();
    }

    [HttpGet("Picture/{id}")]
    public async Task<IActionResult> GetPicture(int id)
    {
        var bytes = await categoryService.GetPictureAsync(id);
        if (bytes == null)
        {
            return NotFound();
        }
        return File(bytes, "image/jpeg");
    }
}
