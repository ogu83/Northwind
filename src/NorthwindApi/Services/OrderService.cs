using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class OrderService(IMapper mapper, IOrderProvider orderProvider)
    : BaseService<Order, DbModels.Order, int>(mapper, orderProvider), IOrderService
{
    public async Task<List<Order>> GetByCustomer(string customerId)
    {
        var dbObj = await orderProvider.GetListByCustomerIdAsync(customerId);
        var retVal = _mapper.Map<List<Order>>(dbObj);
        return retVal;
    }
}
