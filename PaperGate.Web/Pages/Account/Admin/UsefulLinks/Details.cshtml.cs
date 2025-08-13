using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;

namespace PaperGate.Web.Pages.Account.Admin.UsefulLinks
{
    public class DetailsModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
    }
}
