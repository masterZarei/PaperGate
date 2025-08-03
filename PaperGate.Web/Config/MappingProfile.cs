using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Template;
using PaperGate.Web.ViewModels;
using PostGate.Web.ViewModels;

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
            CreateMap<PostInfo, PostCreateDto>().ReverseMap();
            CreateMap<PostInfo, PostListDto>().ReverseMap();
            CreateMap<PostInfo, PostDeleteDto>().ReverseMap();
            CreateMap<PostInfo, PostEditDto>().ReverseMap();
            CreateMap<PostInfo, PostDetailsDto>().ReverseMap();
            CreateMap<PostInfo, PublicPostDetailsDto>().ReverseMap();
            #endregion

            #region Preferences
            CreateMap<AboutUsInfo, AboutUsEditDto>().ReverseMap();
            CreateMap<AboutUsInfo, AboutUsPageDto>().ReverseMap();

            #endregion
        }
    }
}
