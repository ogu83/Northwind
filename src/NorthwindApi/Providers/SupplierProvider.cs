using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class SupplierProvider(InstnwndContext context) : BaseProvider<Supplier>(context), ISupplierProvider
{
    public override async Task<Supplier?> GetByIdAsync(int id) => await Query().FirstOrDefaultAsync(x => x.SupplierId == id);

}