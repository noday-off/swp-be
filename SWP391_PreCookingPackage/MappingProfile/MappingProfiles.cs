using AutoMapper;
using SWP391_PreCookingPackage.Models;
using SWP391_PreCookingPackage.ModelsDTO;

namespace SWP391_PreCookingPackage.MappingProfile
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //User
            CreateMap<UserRegisterModel, User>();
            CreateMap<UserModel, User>();
            CreateMap<User, UserModel>();
            //Author
            CreateMap<AuthorModel, Author>();
            CreateMap<Author, AuthorModel>();
            //Review
            CreateMap<Review, ReviewModel>();
            CreateMap<ReviewModel, Review>();
            //Recipe
            CreateMap<RecipeCreateModel, Recipe>();
            CreateMap<RecipeModel, Recipe>();
            CreateMap<Recipe, RecipeModel>();
            //Package
            CreateMap<PackageModel,Package>();
            CreateMap<Package,PackageModel>();
            //Ingredient
            CreateMap<IngredientModel,Ingredient>();
            CreateMap<Ingredient,IngredientModel>();
            //Category
            CreateMap<Category,CategoryModel>();
            CreateMap<CategoryModel,Category>();
            //Order
            CreateMap<OrderModel,Order>();
            CreateMap<Order,OrderModel>();
            //OrderItem
            CreateMap<OrderItem,OrderItemModel>();
            CreateMap<OrderItemModel,OrderItem>();
            //Recipes Ingredient
            CreateMap<RecipesIngredientModel,RecipesIngredient>();
            CreateMap<RecipesIngredient,RecipesIngredientModel>();

        }
    }
}
