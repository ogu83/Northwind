using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class ProductProvider(InstnwndContext context): BaseProvider<Product>(context), IProductProvider
{
    public override async Task<Product?> GetByIdAsync(int id) => await Query().FirstOrDefaultAsync(x => x.ProductId == id);
    
    public Task<List<Product>> GetListByCategoryIdAsync(int categoryId)
    {
        var retval = Query().Where(x=>x.CategoryId == categoryId).ToListAsync();
        return retval;
    }

    public Task<List<Product>> GetListBySupplierIdAsync(int supplierId)
    {
        var retval = Query().Where(x=>x.SupplierId == supplierId).ToListAsync();
        return retval;
    }
}