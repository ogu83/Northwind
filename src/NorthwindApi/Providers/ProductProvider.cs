using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class ProductProvider(InstnwndContext context): BaseProvider<Product>(context), IProductProvider
{
    public List<Product> GetList() => [.. Query()];

    public List<Product> GetListByCategoryId(int categoryId)
    {
        var retval = Query().Where(x=>x.CategoryId == categoryId).ToList();
        return retval;
    }

    protected override IQueryable<Product> Query() => _context.Products.AsQueryable();
}