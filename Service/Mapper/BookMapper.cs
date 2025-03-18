using AutoMapper;
using Database.Entities;
using Models.DTO;
using System.Linq;

namespace Service.Mapper
{
    public class BookMapper : Profile
    {
        public BookMapper()
        {
            // ✅ Map Book -> BookDto (Include Genre IDs)
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.GenreIds, opt => opt.MapFrom(src => src.Genres.Select(g => g.Id).ToList()));

            // ✅ Map BookDto -> Book (Ignore Genres; Handled in Service Layer)
            CreateMap<BookDto, Book>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore());
        }
    }
}
