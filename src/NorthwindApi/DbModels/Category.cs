using System;
using System.Collections.Generic;
using NorthwindApi.Providers;

namespace NorthwindApi.DbModels;

public partial class Category
{
    public int CategoryId { get; set; }

    [Filterable]
    public string CategoryName { get; set; } = null!;

    // [Filterable]
    public string? Description { get; set; }

    public byte[]? Picture { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
