using NorthwindApi.Models;

namespace NorthwindApi.Services;

public interface IOrderService : IBaseService<Order, int>
{
    Task<List<Order>> GetByCustomer(string customerId);
}
