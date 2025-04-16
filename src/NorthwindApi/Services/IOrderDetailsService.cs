using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface IOrderDetailsService : IBaseService<OrderDetail>
{
    Task<List<OrderDetail>> GetByOrder(int orderId);
}