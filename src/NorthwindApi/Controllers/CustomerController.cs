using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class CustomerController(ICustomerService customerService)
    : EntityApiControllerBase<Customer, ICustomerService>(customerService)
{
    
}