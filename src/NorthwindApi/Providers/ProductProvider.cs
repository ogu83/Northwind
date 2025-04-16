using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class ProductProvider(InstnwndContext context) 
    : BaseProvider<Product, int>(context), IProductProvider
{
    public override async Task<Product?> GetByIdAsync(int id) 
        => await Query().FirstOrDefaultAsync(x => x.ProductId == id);

    public Task<List<Product>> GetListByCategoryIdAsync(int categoryId) 
        => Query().Where(x => x.CategoryId == categoryId).ToListAsync();

    public Task<List<Product>> GetListBySupplierIdAsync(int supplierId)
        => Query().Where(x=>x.SupplierId == supplierId).ToListAsync();
    
}