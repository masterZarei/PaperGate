using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.ViewModels;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages;

public class IndexModel : MyPageModel
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public IndexModel(ILogger logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public IndexDto IndexDto { get; set; }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            string AddOnHeadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", "Html/AddOnHead.html");
            string AddOnBodyPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", "Html/AddOnBody.html");
            string AddOnAfterBodyPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", "Html/AddOnAfterBody.html");


            IndexDto = new IndexDto
            {
                Posts = await _unitOfWork.Post.GetAllReadOnlyAsync(b => b.IsActive, queryCustomizer: b => b.Include(b => b.Author).OrderByDescending(b => b.CreatedOn).Take(6)),
                Sliders = await _unitOfWork.Post.GetAllReadOnlyAsync(p=>p.ShowOnSlider && p.IsActive),
            };
            if (System.IO.File.Exists(AddOnHeadPath))
                IndexDto.AddOnHeadHtml = System.IO.File.ReadAllText(AddOnHeadPath);
            if (System.IO.File.Exists(AddOnBodyPath))
                IndexDto.AddOnBodyHtml = System.IO.File.ReadAllText(AddOnBodyPath);
            if (System.IO.File.Exists(AddOnAfterBodyPath))
                IndexDto.AddOnAfterBodyHtml = System.IO.File.ReadAllText(AddOnAfterBodyPath);
            return Page();

        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "DANGER DANGER DANGER INDEX FAILED ON ONGET", IndexDto);
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return Content("خطایی رخ داد");
        }

    }
}
