using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class OrderDetailsController(IOrderDetailService orderDetailsService)
    : EntityApiControllerBase<OrderDetail, IOrderDetailService, Tuple<int, int>>(orderDetailsService)
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

    [HttpGet("{orderID}/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDetail>> Get(int orderID, int productId)
    {
        var retVal = await orderDetailsService.GetById(new Tuple<int, int>(orderID, productId));
        if (retVal == null)
            return NotFound();
        return new ActionResult<OrderDetail>(retVal);
    }

    /// <summary>
    /// Hide the inherited Get(id)
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [NonAction]
    public override Task<ActionResult<OrderDetail>> Get(Tuple<int, int> id)
    {
        // we could throw NotSupportedException() or simply return 404:
        return Task.FromResult<ActionResult<OrderDetail>>(NotFound());
    }
}
