using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<RecipesCategory> RecipesCategories { get; set; } = new List<RecipesCategory>();
}
