using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class OrderDetailsProvider(InstnwndContext context) : BaseProvider<OrderDetail>(context), IOrderDetailsProvider
{
    public Task<List<OrderDetail>> GetListByOrderId(int orderId) 
        => Query().Where(x => x.OrderId == orderId).ToListAsync();
}
