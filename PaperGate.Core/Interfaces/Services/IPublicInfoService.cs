using PaperGate.Core.DTOs;
using PaperGate.Core.ViewModels;

namespace PaperGate.Core.Interfaces.Services;
public interface IPublicInfoService
{
    Task<AboutUsPageDto?> GetAboutUsInfoAsync();
    Task<FooterDto?> GetFooterInfoAsync();
    Task<AllPostsDto?> GetAllPostsInfoAsync(int sub, string? searchTitle = null);
}
