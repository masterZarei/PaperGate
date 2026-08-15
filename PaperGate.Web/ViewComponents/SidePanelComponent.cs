using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Interfaces;
using PaperGate.Web.ViewModels;

namespace PaperGate.Web.ViewComponenets;

public class SidePanelComponent : ViewComponent
{
    private readonly IUnitOfWork _unitOfWork;

    public SidePanelComponent(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new SidePanelViewModel
        {
            Categories = await _unitOfWork.Category.GetAllReadOnlyAsync()
        };
        return View("/Pages/Shared/Components/Panel/_Panel.cshtml", model);

    }
}
