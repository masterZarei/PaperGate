using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;

namespace PaperGate.Web.Pages.Account.Admin.Messages
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public IList<MessageInfo> MessageInfo { get; set; }
        public async Task<IActionResult> OnGet()
        {
            MessageInfo = await _unitOfWork.Message.GetAllAsync(queryCustomizer: q => q.OrderBy(m => m.Read).ThenBy(m => m.CreatedOn));
            return Page();
        }
    }
}
