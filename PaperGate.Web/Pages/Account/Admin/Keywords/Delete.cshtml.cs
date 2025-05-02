using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.Keywords
{
    public class DeleteModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public DeleteModel(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [BindProperty]
        public KeywordInfo KeywordDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToPage("./Index");
            }

            KeywordDTO = await _unitOfWork.Keyword.GetAsync(m => m.Id == id);

            if (KeywordDTO is null)
            {
                ShowError(ErrorMessages.NOTFOUND);
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id <= 0)
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToLocalIndex();
            }
            try
            {
                var KeywordDTO = await _unitOfWork.Keyword.GetAsync(c => c.Id == id);
                if (KeywordDTO != null)
                {
                    await _unitOfWork.Keyword.RemoveAsync(KeywordDTO);
                    await _unitOfWork.SaveChangesAsync();
                }
                ShowSuccess();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "Error occured in Delete Page of Keyword Management on OnPostAsync");
                return RedirectToLocalIndex();
            }

        }
    }
}
