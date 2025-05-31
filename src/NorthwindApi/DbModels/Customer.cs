using System;
using System.Collections.Generic;
using NorthwindApi.Providers;

namespace NorthwindApi.DbModels;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    [Filterable]
    public string CompanyName { get; set; } = null!;

    [Filterable]
    public string? ContactName { get; set; }

    [Filterable]
    public string? ContactTitle { get; set; }

    [Filterable]
    public string? Address { get; set; }

    [Filterable]
    public string? City { get; set; }

    [Filterable]
    public string? Region { get; set; }

    [Filterable]
    public string? PostalCode { get; set; }

    [Filterable]
    public string? Country { get; set; }

    [Filterable]
    public string? Phone { get; set; }

    [Filterable]
    public string? Fax { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<CustomerDemographic> CustomerTypes { get; set; } = new List<CustomerDemographic>();
}
