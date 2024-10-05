using AutoMapper;
using RoleWebApi.Dto;
using RoleWebApi.Models;

namespace RoleWebApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Role, RoleAddDTO>().ReverseMap();
        }
    }
}
