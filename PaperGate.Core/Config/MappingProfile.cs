using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;

namespace PaperGate.Core.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User_DTOs
            CreateMap<IdentityUser, UserInfo>().ReverseMap();
            CreateMap<UserInfo, UserCreateDto>().ReverseMap();
            CreateMap<UserInfo, UserListDto>().ReverseMap();
            CreateMap<UserInfo, UserDeleteDto>().ReverseMap();
            CreateMap<UserInfo, User_ED_Dto>().ReverseMap();
            CreateMap<UserInfo, PersonalInfoDto>().ReverseMap();
            #endregion
        }
    }
}
