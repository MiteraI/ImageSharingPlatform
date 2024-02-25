using AutoMapper;
using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Dto;

namespace ImageSharingPlatform.Configuration.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserEditDto>()
                .ForMember(dest => dest.Avatar, opt => opt.Ignore()).ReverseMap();
        }
    }
}
