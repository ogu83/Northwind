using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers;

public class SupplierController(ISupplierService supplierService, ILoggerFactory logger)
    : EntityApiControllerBase<Supplier, ISupplierService, int>(supplierService, logger)
{

}