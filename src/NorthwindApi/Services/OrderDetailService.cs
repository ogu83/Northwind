using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class OrderDetailService(IMapper mapper, IOrderDetailsProvider orderDetailsProvider)
    : BaseService<OrderDetail, DbModels.OrderDetail, Tuple<int, int>>(mapper, orderDetailsProvider), IOrderDetailService
{
    public async Task<List<OrderDetail>> GetByOrder(int orderId)
    {
        var dbObj = await orderDetailsProvider.GetListByOrderId(orderId);
        var retVal = _mapper.Map<List<OrderDetail>>(dbObj);
        return retVal;
    }
}
