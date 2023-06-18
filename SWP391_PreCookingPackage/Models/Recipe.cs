using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class Recipe
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Instructions { get; set; }

    public string? PreserveAdvice { get; set; }

    public string? Image { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    public virtual ICollection<RecipesIngredient> RecipesIngredients { get; set; } = new List<RecipesIngredient>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
