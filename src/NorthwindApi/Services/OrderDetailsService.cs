using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class OrderDetailsService(IMapper mapper, IOrderDetailsProvider orderDetailsProvider)
    : BaseService<OrderDetail, DbModels.OrderDetail>(mapper, orderDetailsProvider), IOrderDetailsService
{
    public async Task<List<OrderDetail>> GetByOrder(int orderId)
    {
        var dbObj = await orderDetailsProvider.GetListByOrderId(orderId);
        var retVal = _mapper.Map<List<OrderDetail>>(dbObj);
        return retVal;
    }
}
