using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ContactInfo { get; set; }

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}
