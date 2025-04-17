using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class CategoryController(ICategoryService categoryService) 
    : EntityApiControllerBase<Category, ICategoryService, int>(categoryService)
{
    /// <summary>
    /// Returns the picture of the category as a mime type of image/bmp
    /// </summary>
    /// <param name="id">Id of the Category</param>
    /// <returns>The Image of the category.</returns>
    [HttpGet("Picture/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPicture(int id)
    {
        var bytes = await categoryService.GetPictureAsync(id);
        if (bytes == null)
        {
            return NotFound();
        }
        return File(bytes, "image/bmp");
    }
}
