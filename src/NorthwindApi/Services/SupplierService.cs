using AutoMapper;
using NorthwindApi.Models;
using NorthwindApi.Providers;

namespace NorthwindApi.Services;

public class SupplierService(IMapper mapper, ISupplierProvider supplierProvider)
    : BaseService<Supplier, DbModels.Supplier>(mapper, supplierProvider), ISupplierService
{
    
}