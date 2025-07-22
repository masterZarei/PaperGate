using Microsoft.AspNetCore.Mvc.RazorPages;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Interfaces;

namespace PaperGate.Web.Pages.Account.Admin.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IReadOnlyList<CategoryInfo> CategoryDTO { get; set; }
        public async Task OnGet()
        {
            CategoryDTO = await _unitOfWork.Category.GetAllReadOnlyAsync();
        }
    }
}
