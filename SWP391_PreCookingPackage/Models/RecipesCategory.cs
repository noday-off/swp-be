using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class RecipesCategory
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
