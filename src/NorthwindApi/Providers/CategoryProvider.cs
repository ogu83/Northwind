using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class CategoryProvider(InstnwndContext context) 
    : BaseProvider<Category, int>(context), ICategoryProvider
{
    protected IQueryable<Category> QueryWithProducts() 
        => Query().Include(x => x.Products);
}