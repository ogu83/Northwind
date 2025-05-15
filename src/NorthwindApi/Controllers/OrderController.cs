using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class OrderController(IOrderService service, ILoggerFactory loggerFactory)
    : EntityApiControllerBase<Order, IOrderService, int>(service, loggerFactory)
{
    private readonly IOrderService _orderService = service;

    /// <summary>
    /// Returns the Orders of the specified Customer.
    /// </summary>
    /// <param name="id">Customer Id</param>
    /// <returns>Customers orders.</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("Customer/{id}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByCustomer(string id)
    {
        var retval = await _orderService.GetByCustomer(id);
        return retval;
    }
}
