using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class CategoryProvider(InstnwndContext context) : BaseProvider<Category>(context), ICategoryProvider
{
    public List<Category> GetList() => [.. Query()];

    protected override IQueryable<Category> Query() => _context.Categories.AsQueryable();

    protected IQueryable<Category> QueryWithProducts() => Query().Include(x => x.Products);

    public override Category? GetById(int id) => Query().FirstOrDefault(x => x.CategoryId == id);
}
