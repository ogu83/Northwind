using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class CategoryController(ICategoryService categoryService) : ApiControllerBase
{
    [HttpGet]
    public IEnumerable<Category> Get()
    {
        var retVal = categoryService.GetCategoryList();
        return retVal;
    }
}