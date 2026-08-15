using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Interfaces.Services;

namespace Emzacode.Web.ViewComponents;

public class FooterComponent : ViewComponent
{
    private readonly IPublicInfoService _publicInfoService;

    public FooterComponent(IPublicInfoService publicInfoService)
    {
        _publicInfoService = publicInfoService;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("/Pages/Shared/Components/Footer/_FooterViewComponent.cshtml"
            , await _publicInfoService.GetFooterInfoAsync());
    }
}
