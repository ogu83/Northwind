using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class CategoryController(ICategoryService categoryService) 
    : EntityApiControllerBase<Category, ICategoryService>(categoryService)
{
    [HttpGet("Picture/{id}")]
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
