using System;
using System.Collections.Generic;

namespace SWP391_PreCookingPackage.Models;

public partial class Review
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RecipeId { get; set; }

    public string Title { get; set; } = null!;

    public string? Detail { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
