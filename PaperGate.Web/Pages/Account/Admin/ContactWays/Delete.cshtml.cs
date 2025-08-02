using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.ContactWays
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    ShowError(ErrorMessages.IDINVALID); 
                    return RedirectToLocalIndex();
                }

                var contactwayinfo = await _unitOfWork.ContactWay.GetAsync(c => c.Id == id);
                if (contactwayinfo != null)
                {
                    ContactWayInfo = contactwayinfo;
                    await _unitOfWork.ContactWay.RemoveAsync(ContactWayInfo);
                    await _unitOfWork.SaveChangesAsync();
                }
                ShowSuccess();
                return RedirectToLocalIndex();
            }
            catch (Exception ex)
            {
                ShowError(ErrorMessages.CUSTOM, customMessage: "در فرآیند حذف راه ارتباطی خطایی رخ داد. لطفا بعدا امتحان کنید");
                _logger.Fatal(ex, ex.Message, "حذف راه ارتباطی با خطا مواجه شد");
                return Page();
            }

        }
    }
}
