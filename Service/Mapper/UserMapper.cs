using AutoMapper;
using Database.Entities;
using Models.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserResponseDto>();
        }
    }
}
