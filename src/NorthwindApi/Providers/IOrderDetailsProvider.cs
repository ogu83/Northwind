using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface IOrderDetailsProvider : IBaseProvider<OrderDetail, int>
{
    Task<List<OrderDetail>> GetListByOrderId(int orderId);
}
