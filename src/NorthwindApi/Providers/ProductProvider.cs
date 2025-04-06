using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class ProductProvider(InstnwndContext context): BaseProvider<Product>(context), IProductProvider
{
    public Task<List<Product>> GetListAsync() => Query().ToListAsync();

    public Task<List<Product>> GetListByCategoryIdAsync(int categoryId)
    {
        var retval = Query().Where(x=>x.CategoryId == categoryId).ToListAsync();
        return retval;
    }

    protected override IQueryable<Product> Query() => _context.Products.AsQueryable();
}