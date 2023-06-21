using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class Package
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public string? Title { get; set; }

    public string? Detail { get; set; }

    public string? NutritionFacts { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public int? Sales { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Recipe Recipe { get; set; } = null!;
}
