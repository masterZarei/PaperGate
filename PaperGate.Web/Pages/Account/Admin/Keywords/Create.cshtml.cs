using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.Keywords
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
        public KeywordInfo KeywordDTO { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ShowError(ErrorMessages.FILLREQUESTEDDATA);
                return Page(KeywordDTO);
            }
            try
            {
                await _unitOfWork.Keyword.AddAsync(KeywordDTO);
                await _unitOfWork.SaveChangesAsync();
                ShowSuccess();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occured in Create Page of Keyword Management on OnPostAsync");
                return RedirectToPage("./index");
            }

        }
    }
}
