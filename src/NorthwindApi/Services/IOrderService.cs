using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface IOrderService : IBaseService<Order>
{
    Task<List<Order>> GetByCustomer(string customerId);
}
