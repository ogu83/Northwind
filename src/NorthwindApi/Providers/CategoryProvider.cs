using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class CategoryProvider(InstnwndContext context) : BaseProvider<Category>(context), ICategoryProvider
{
    public override async Task<Category?> GetByIdAsync(int id) => await Query().FirstOrDefaultAsync(x => x.CategoryId == id);

    protected IQueryable<Category> QueryWithProducts() => Query().Include(x => x.Products);
}