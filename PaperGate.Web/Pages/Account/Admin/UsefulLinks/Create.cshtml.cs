using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.UsefulLinks
{
    public class CreateModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CreateModel(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UsefulLinkInfo UsefulLinkDto { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ShowError(ErrorMessages.FILLREQUESTEDDATA);
                return Page(UsefulLinkDto);
            }
            try
            {
                await _unitOfWork.UsefulLink.AddAsync(UsefulLinkDto);
                await _unitOfWork.SaveChangesAsync();
                ShowSuccess();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occured in Create Page of UsefulLink Management on OnPostAsync");
                return RedirectToPage("./index");
            }

        }
    }
}
