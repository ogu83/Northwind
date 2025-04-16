using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface IOrderProvider : IBaseProvider<Order, int> 
{
    Task<List<Order>> GetListByCustomerIdAsync(string customerId);
}
