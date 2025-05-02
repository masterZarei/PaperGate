using Microsoft.AspNetCore.Mvc.RazorPages;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Interfaces;

namespace PaperGate.Web.Pages.Account.Admin.Keywords
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IReadOnlyList<KeywordInfo> KeywordDTO { get; set; }
        public async Task OnGet()
        {
            KeywordDTO = await _unitOfWork.Keyword.GetAllAsync();
        }
    }
}
