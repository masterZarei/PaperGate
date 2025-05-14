using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.Categories
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
        public CategoryInfo CategoryDTO { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id <= 0)
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToLocalIndex();
            }
            try
            {
                var CategoryDTO = await _unitOfWork.Category.GetAsync(c => c.Id == id);
                if (CategoryDTO != null)
                {
                    await _unitOfWork.Category.RemoveAsync(CategoryDTO);
                    await _unitOfWork.SaveChangesAsync();
                }
                ShowSuccess();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "Error occured in Delete Page of Category Management on OnPostAsync");
                return RedirectToLocalIndex();
            }

        }
    }
}
