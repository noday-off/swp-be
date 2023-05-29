using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class Delivery
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string? ShippperName { get; set; }

    public DateTime? ShipDate { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Order Order { get; set; } = null!;
}
