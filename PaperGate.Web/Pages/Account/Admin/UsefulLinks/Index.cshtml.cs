using Microsoft.AspNetCore.Mvc.RazorPages;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;

namespace PaperGate.Web.Pages.Account.Admin.UsefulLinks
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IReadOnlyList<UsefulLinkInfo> UsefulLinkDTO { get; set; }
        public async Task OnGet()
        {
            UsefulLinkDTO = await _unitOfWork.UsefulLink.GetAllReadOnlyAsync();
        }
    }
}
