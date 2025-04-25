using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface IOrderDetailsProvider : IBaseProvider<OrderDetail, Tuple<int, int>>
{
    Task<List<OrderDetail>> GetListByOrderId(int orderId);
}
