using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface IOrderDetailService : IBaseService<OrderDetail, Tuple<int,int>>
{
    Task<List<OrderDetail>> GetByOrder(int orderId);
}