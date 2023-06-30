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

            //Ingredient
            CreateMap<Ingredient, IngredientModel>();
            CreateMap<IngredientModel, Ingredient>();

        }
    }
}
