using Microsoft.AspNetCore.Localization;
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
            IndexDto = new IndexDto
            {
               // Posts = await _unitOfWork.Post.GetAllReadOnlyAsync(b => b.IsActive, queryCustomizer: b => b.Include(b => b.Category).OrderByDescending(b => b.CreatedOn)),
                Sliders = await _unitOfWork.Post.GetAllReadOnlyAsync(p => p.ShowOnSlider && p.IsActive),
                Categories = await _unitOfWork.Category.GetAllReadOnlyAsync(c => c.Posts.Count > 0, queryCustomizer: q => q.Include(c => c.Posts))
            };
            return Page();

        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "DANGER DANGER DANGER INDEX FAILED ON ONGET", IndexDto);
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return Content("خطایی رخ داد");
        }

    }
    public IActionResult OnPostChangeLanguage(string culture)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

        ShowSuccess($"زبان سایت به {(culture=="fa" ? "فارسی" : "انگلیسی")} تغییر کرد.");
        return RedirectToPage();
    }
}
