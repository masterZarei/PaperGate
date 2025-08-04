using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.Utilities.Libraries;
using System.Runtime.CompilerServices;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.ContactWays
{
    public class EditModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public EditModel(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [BindProperty]
        public ContactWayInfo ContactWayInfo { get; set; } = default!;
        public SelectList Icons { get; set; }

        [BindProperty]
        public string SelectedIcon { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToLocalIndex();
            }

            var contactwayinfo =  await _unitOfWork.ContactWay.GetAsync(m => m.Id == id);
            if (contactwayinfo == null)
            {
                ShowError(ErrorMessages.NOTFOUND);
                return RedirectToLocalIndex();
            }
            ContactWayInfo = contactwayinfo;
            Init(ContactWayInfo.Icon);
            return Page();
        }
        void Init(string? currentIcon)
        {
            Icons = new SelectList(IconCollection.GetIcons, "Value", "Name",currentIcon);
        }
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ShowError(ErrorMessages.FILLREQUESTEDDATA);
                    return Page(ContactWayInfo);
                }
                ContactWayInfo.Icon = SelectedIcon;
                await _unitOfWork.ContactWay.UpdateAsync(ContactWayInfo);
                await _unitOfWork.SaveChangesAsync();

                ShowSuccess();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ShowError(ErrorMessages.CUSTOM, customMessage: "در فرآیند ویرایش راه ارتباطی خطایی رخ داد. لطفا بعدا امتحان کنید");
                _logger.Fatal(ex, ex.Message, "ویرایش راه ارتباطی با خطا مواجه شد");
                return Page();
            }

        }
    }
}
