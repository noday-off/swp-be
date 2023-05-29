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

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    public virtual ICollection<RecipesCategory> RecipesCategories { get; set; } = new List<RecipesCategory>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
