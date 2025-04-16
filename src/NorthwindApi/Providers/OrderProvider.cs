using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class OrderProvider(InstnwndContext context) 
    : BaseProvider<Order, int>(context), IOrderProvider
{
    public override async Task<Order?> GetByIdAsync(int id) 
        => await Query().FirstOrDefaultAsync(x => x.OrderId == id);

    public Task<List<Order>> GetListByCustomerIdAsync(string customerId) 
        => Query().Where(x => x.CustomerId == customerId).ToListAsync();
}
