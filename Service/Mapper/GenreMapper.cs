using AutoMapper;
using Database.Entities;
using Models.DTO;

namespace Service.Mapper
{
    public class GenreMapper : Profile
    {
        public GenreMapper()
        {
            CreateMap<Genre, GenreDto>().ReverseMap();
        }
    }
}
