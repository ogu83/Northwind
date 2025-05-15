using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class CustomerController(ICustomerService customerService, ILoggerFactory loggerFactory)
    : EntityApiControllerBase<Customer, ICustomerService, string>(customerService, loggerFactory)
{

}