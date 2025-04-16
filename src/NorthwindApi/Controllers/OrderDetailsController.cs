using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class OrderDetailsController(IOrderDetailService orderDetailsService)
    : EntityApiControllerBase<OrderDetail, IOrderDetailService, int>(orderDetailsService)
{
    [HttpGet("Order/{id}")]
    public async Task<ActionResult<IEnumerable<OrderDetail>>> GetByOrder(int id)
    {
        var retval = await orderDetailsService.GetByOrder(id);
        return retval;
    }
}
