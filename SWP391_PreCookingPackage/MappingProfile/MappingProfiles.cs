using AutoMapper;
using SWP391_PreCookingPackage.Models;
using SWP391_PreCookingPackage.ModelsDTO;

namespace SWP391_PreCookingPackage.MappingProfile
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Author
            CreateMap<AuthorModel, Author>();
            CreateMap<Author, AuthorModel>();
            //Review
            CreateMap<Review, ReviewModel>();
            CreateMap<ReviewModel, Review>();
        }
    }
}
