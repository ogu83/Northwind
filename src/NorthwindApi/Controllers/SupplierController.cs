using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class SupplierController(ISupplierService supplierService)
    : EntityApiControllerBase<Supplier, ISupplierService, int>(supplierService)
{
    
}