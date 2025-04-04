using System;
using System.Collections.Generic;

namespace NorthwindApi.DbModels;

public partial class Category1
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string Description { get; set; } = null!;
}
