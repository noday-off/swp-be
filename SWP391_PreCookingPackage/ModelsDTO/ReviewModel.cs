using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public class ReviewModel
{
	public int? Id { get; set; }
	public int UserId { get; set; }
	public int RecipeId { get; set; }
	public string? UserName { get; set; }
    public string Title { get; set; } = null!;
    public string Detail { get; set; } = null!;
}

