using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages
{
    public class ContactUsModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ContactUsModel(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [BindProperty]
        public MessageInfo MessageDto { get; set; }

        [BindProperty]
        public IReadOnlyCollection<PostInfo> LastestPosts { get; set; }
        public async Task<IActionResult> OnGet()
        {
            LastestPosts = await _unitOfWork.Post.GetAllReadOnlyAsync();
            return Page();
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
                    ShowSuccess("پیام شما با موفقیت ارسال شد");
                }
                return RedirectToIndex();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "DANGER DANGER DANGER ContactUs FAILED ON OnGet", MessageDto);
                ShowWarning("فرآیند ارسال پیام انجام نشد.");
                return RedirectToIndex();
            }
        }
    }
}
