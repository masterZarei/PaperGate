using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;

namespace PaperGate.Web.Pages.Account.Admin.Keywords
{
    public class DetailsModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
    }
}
