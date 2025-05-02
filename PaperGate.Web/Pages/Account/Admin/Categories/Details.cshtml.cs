using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;

namespace PaperGate.Web.Pages.Account.Admin.Categories
{
    public class DetailsModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CategoryInfo CategoryDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id  <= 0)
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToPage("./Index");
            }

            CategoryDTO = await _unitOfWork.Category.GetAsync(m => m.Id == id);

            if (CategoryDTO is null)
            {
                ShowError(ErrorMessages.NOTFOUND);
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
