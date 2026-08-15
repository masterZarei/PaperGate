using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;
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
        public MessageCreateDto MessageDto { get; set; }

        [BindProperty]
        public IReadOnlyCollection<PostInfo> LastestPosts { get; set; }
        public async Task<IActionResult> OnGet()
        {
            LastestPosts = await _unitOfWork.Post.GetAllReadOnlyAsync(queryCustomizer: q => q.Where(p => p.IsActive).Take(6));
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

                var message = new MessageInfo
                {
                    SendersName = MessageDto.SendersName ?? string.Empty,
                    SendersEmail = MessageDto.SendersEmail ?? string.Empty,
                    Content = MessageDto.Content ?? string.Empty,
                    Read = false,
                };

                await _unitOfWork.Message.AddAsync(message);
                await _unitOfWork.SaveChangesAsync();

                ShowSuccess("پیام شما با موفقیت ارسال شد");
                return RedirectToIndex();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "ContactUs OnPost Failed");
                ShowWarning("فرآیند ارسال پیام انجام نشد.");
                return RedirectToIndex();
            }
        }
    }
}
