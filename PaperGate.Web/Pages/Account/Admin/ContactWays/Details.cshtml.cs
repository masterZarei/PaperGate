using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.ContactWays
{
    public class DetailsModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public DetailsModel(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public ContactWayInfo ContactWayInfo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToLocalIndex();
            }

            var contactwayinfo = await _unitOfWork.ContactWay.GetAsync(m => m.Id == id);

            if (contactwayinfo is not null)
            {
                ContactWayInfo = contactwayinfo;

                return Page();
            }
            ShowError(ErrorMessages.NOTFOUND);
            return RedirectToLocalIndex();
        }
    }
}
