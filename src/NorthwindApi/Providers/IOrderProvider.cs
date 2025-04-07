using NorthwindApi.DbModels;

namespace NorthwindApi.Providers;

public interface IOrderProvider : IBaseProvider<Order> 
{
    Task<List<Order>> GetListByCustomerIdAsync(string customerId);
}
