using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;
using PaperGate.Web.ViewModels;

namespace PaperGate.Web.Config
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

            #region Paper_DTOs
            CreateMap<PaperInfo, PaperCreateDto>().ReverseMap();
            CreateMap<PaperInfo, PaperListDto>().ReverseMap();
            CreateMap<PaperInfo, PaperDeleteDto>().ReverseMap();
            CreateMap<PaperInfo, PaperEditDto>().ReverseMap();
            CreateMap<PaperInfo, PaperDetailsDto>().ReverseMap();
            CreateMap<PaperInfo, PublicPaperDetailsDto>().ReverseMap();
            #endregion
        }
    }
}
