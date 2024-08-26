using AutoMapper;
using destapp.api.Models;
using destapp.biz.Entities;

namespace destapp.api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            #endregion

        }
    }
}