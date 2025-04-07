using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface IOrderDetailsProvider : IBaseProvider<OrderDetail>
{
    Task<List<OrderDetail>> GetListByOrderId(int orderId);
}
