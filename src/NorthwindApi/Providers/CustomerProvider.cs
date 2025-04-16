using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class CustomerProvider(InstnwndContext context) 
    : BaseProvider<Customer, string>(context), ICustomerProvider
{
    public override async Task<Customer?> GetByIdAsync(string id) 
        => await Query().FirstOrDefaultAsync(x => x.CustomerId == id);
}