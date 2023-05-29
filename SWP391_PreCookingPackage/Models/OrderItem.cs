using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int PackageId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Package Package { get; set; } = null!;
}
