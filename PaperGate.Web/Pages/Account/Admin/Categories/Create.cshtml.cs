using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.Categories
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
        public CategoryInfo CategoryDTO { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ShowError(ErrorMessages.FILLREQUESTEDDATA);
                return Page(CategoryDTO);
            }
            try
            {
                await _unitOfWork.Category.AddAsync(CategoryDTO);
                await _unitOfWork.SaveChangesAsync();
                ShowSuccess();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occured in Create Page of Category Management on OnPostAsync");
                return RedirectToPage("./index");
            }

        }
    }
}
