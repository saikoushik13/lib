using AutoMapper;
using Database.Entities;
using Models.DTO;

namespace Service.Mapper
{
    public class ReviewMapper : Profile
    {
        public ReviewMapper()
        {
            // ✅ Map Review -> ReviewDto (Include Username from User Entity)
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));

            // ✅ Map ReviewCreateDto -> Review (Ignore User/Book References, Handled in Service Layer)
            CreateMap<ReviewCreateDto, Review>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore());

            // ✅ Map ReviewUpdateDto -> Review
            CreateMap<ReviewUpdateDto, Review>();
        }
    }
}
