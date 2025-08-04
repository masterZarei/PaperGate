using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.Utilities.Libraries;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.ContactWays
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
            Init();
            return Page();
        }
        void Init()
        {
            Icons = new SelectList(IconCollection.GetIcons, "Value", "Name");
        }

        [BindProperty]
        public ContactWayInfo ContactWayInfo { get; set; } = default!;

        public SelectList Icons { get; set; }

        [BindProperty]
        public string SelectedIcon { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Init();
                    ShowError(ErrorMessages.FILLREQUESTEDDATA);
                    return Page(ContactWayInfo);
                }
                ContactWayInfo.Icon = SelectedIcon;
                await _unitOfWork.ContactWay.AddAsync(ContactWayInfo);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ShowError(ErrorMessages.CUSTOM, customMessage: "در فرآیند ایجاد راه ارتباطی خطایی رخ داد. لطفا بعدا امتحان کنید");
                _logger.Fatal(ex, ex.Message, "ایجاد راه ارتباطی با خطا مواجه شد");
                return Page();
            }

        }
    }
}
