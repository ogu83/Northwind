using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public class OrderDetailsProvider(InstnwndContext context) 
    : BaseProvider<OrderDetail, Tuple<int, int>>(context), IOrderDetailsProvider
{
    public Task<List<OrderDetail>> GetListByOrderId(int orderId) 
        => Query().Where(x => x.OrderId == orderId).ToListAsync();

    protected override async Task<OrderDetail?> FindAsync(Tuple<int, int> id)
    {
        var result = await _context.OrderDetails.FindAsync(id.Item1, id.Item2);
        return result;
    }
}
