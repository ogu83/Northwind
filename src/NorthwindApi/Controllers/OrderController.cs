using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class OrderController(IOrderService orderService)
    : EntityApiControllerBase<Order, IOrderService, int>(orderService)
{
    [HttpGet("Customer/{id}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByCustomer(string id)
    {
        var retval = await orderService.GetByCustomer(id);
        return retval;
    }
}
