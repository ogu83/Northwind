using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class CategoryProvider(InstnwndContext context) : BaseProvider<Category>(context), ICategoryProvider
{
    public Task<List<Category>> GetListAsync() => Query().ToListAsync();

    protected override IQueryable<Category> Query() => _context.Categories.AsQueryable();

    protected IQueryable<Category> QueryWithProducts() => Query().Include(x => x.Products);

    public override async Task<Category?> GetByIdAsync(int id) => await Query().FirstOrDefaultAsync(x => x.CategoryId == id);
}
