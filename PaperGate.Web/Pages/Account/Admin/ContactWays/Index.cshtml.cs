using Microsoft.AspNetCore.Mvc.RazorPages;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;

namespace PaperGate.Web.Pages.Account.Admin.ContactWays
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IReadOnlyList<ContactWayInfo> ContactWayInfo { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ContactWayInfo = await _unitOfWork.ContactWay.GetAllReadOnlyAsync(queryCustomizer: q => q.OrderByDescending(c => c.CreatedOn));
        }
    }
}
