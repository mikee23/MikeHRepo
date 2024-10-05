using AutoMapper;
using AutoMapper.Execution;
using PhotoWebApi.Dto;
using PhotoWebApi.Models;
using System.Data;

namespace PhotoWebApi.Mapper
{
    public class  MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Photo, PhotoAddDTO>().ReverseMap();
        }
    }
}
