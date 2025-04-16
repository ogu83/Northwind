using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class CustomerService(IMapper mapper, ICustomerProvider customerProvider)
    : BaseService<Customer, DbModels.Customer, string>(mapper, customerProvider), ICustomerService
{
       
}