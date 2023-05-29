using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class Author
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Email { get; set; }

    public string? Contact { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
