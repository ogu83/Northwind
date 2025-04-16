using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface IOrderDetailService : IBaseService<OrderDetail, int>
{
    Task<List<OrderDetail>> GetByOrder(int orderId);
}