using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class RecipesIngredient
{
    public int Id { get; set; }

    public int IngredientId { get; set; }

    public int RecipeId { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
