using PaperGate.Core.DTOs;
using PaperGate.Core.ViewModels;

namespace PaperGate.Core.Interfaces.Services;
public interface IPublicInfoService
{
    public Task<AboutUsPageDto> GetAboutUsInfoAsync();
    public Task<FooterDto> GetFooterInfoAsync();
    public Task<AllPostsDto> GetAllPostsInfoAsync(int sub);

}
