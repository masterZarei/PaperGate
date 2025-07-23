using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Core.Interfaces.Repositories;
using PaperGate.Web.Utilities.Helpers;
using System.Text.Json;

namespace PaperGate.Web.Pages
{
    public class ContactUsModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMyLoggerRepository _myLogger;

        public ContactUsModel(IUnitOfWork unitOfWork, IMyLoggerRepository myLogger)
        {
            _unitOfWork = unitOfWork;
            _myLogger = myLogger;
        }
        [BindProperty]
        public MessageInfo MessageDto { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ShowError(ErrorMessages.FILLREQUESTEDDATA);
                    return RedirectToIndex();
                }
                else
                {
                    await _unitOfWork.Message.AddAsync(MessageDto);
                    await _unitOfWork.SaveChangesAsync();

                    string message = $"New Message has been received {JsonSerializer.Serialize(MessageDto)}";
                    await _myLogger.Log("ContactUs", message, LoggingLevel.Information);
                    ShowSuccess("پیام شما با موفقیت ارسال شد");
                }
                return RedirectToIndex();
            }
            catch
            {
                ShowWarning("فرآیند ارسال پیام انجام نشد.");
                return RedirectToIndex();
            }
        }
    }
}
