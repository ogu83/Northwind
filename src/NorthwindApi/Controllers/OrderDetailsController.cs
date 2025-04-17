using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class OrderDetailsController(IOrderDetailService orderDetailsService)
    : EntityApiControllerBase<OrderDetail, IOrderDetailService, int>(orderDetailsService)
{
    /// <summary>
    /// Returns order details of an order.
    /// </summary>
    /// <param name="id">Order Id</param>
    /// <returns>Order Detail List of the order</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("Order/{id}")]
    public async Task<ActionResult<IEnumerable<OrderDetail>>> GetByOrder(int id)
    {
        var retval = await orderDetailsService.GetByOrder(id);
        return retval;
    }
}
