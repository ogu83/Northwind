﻿using System;
using System.Collections.Generic;

namespace NorthwindApi.DbModels;

public partial class OrderSubtotal
{
    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }
}
