using System;
using System.Collections.Generic;
using NorthwindApi.Providers;

namespace NorthwindApi.DbModels;

public partial class Order
{
    public int OrderId { get; set; }

    public string? CustomerId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public int? ShipVia { get; set; }

    public decimal? Freight { get; set; }

    [Filterable]
    public string? ShipName { get; set; }

    [Filterable]
    public string? ShipAddress { get; set; }

    [Filterable]
    public string? ShipCity { get; set; }

    [Filterable]
    public string? ShipRegion { get; set; }

    [Filterable]
    public string? ShipPostalCode { get; set; }

    [Filterable]
    public string? ShipCountry { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Shipper? ShipViaNavigation { get; set; }
}
