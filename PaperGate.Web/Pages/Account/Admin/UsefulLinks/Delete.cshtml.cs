using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.UsefulLinks
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
        public UsefulLinkInfo UsefulLinkDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToPage("./Index");
            }

            UsefulLinkDTO = await _unitOfWork.UsefulLink.GetAsync(m => m.Id == id);

            if (UsefulLinkDTO is null)
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
                var UsefulLinkDTO = await _unitOfWork.UsefulLink.GetAsync(c => c.Id == id);
                if (UsefulLinkDTO != null)
                {
                    await _unitOfWork.UsefulLink.RemoveAsync(UsefulLinkDTO);
                    await _unitOfWork.SaveChangesAsync();
                }
                ShowSuccess();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "Error occured in Delete Page of UsefulLink Management on OnPostAsync");
                return RedirectToLocalIndex();
            }

        }
    }
}
