using SWP391_PreCookingPackage.Models;

namespace SWP391_PreCookingPackage.ModelsDTO
{
    public class RecipeModel
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Instructions { get; set; }

        public string? PreserveAdvice { get; set; }

        public string? Image { get; set; }
        public List<string> Ingredients { get; set; } = new List<string>();
        public List<ReviewModel> Reviews { get; set; } = new List<ReviewModel>();
        public List<PackageModel> Packages { get; set; } = new List<PackageModel>();
    }

    public class RecipeCreateModel
    {
        public int? Id { get; set; }
        public int? AuthorId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Instructions { get; set; }

        public string? PreserveAdvice { get; set; }

        public string? Image { get; set; }
    }

    public class PackageModel
    {
        public int? Id { get; set; }

        public int RecipeId { get; set; }

        public string? Title { get; set; }

        public string? Detail { get; set; }

        public string? NutritionFacts { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public int? Sales { get; set; }
    }
}
