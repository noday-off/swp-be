﻿using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class Category
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public string? Name { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;
}
