﻿using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class Ingredient
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<RecipesIngredient> RecipesIngredients { get; set; } = new List<RecipesIngredient>();
}
