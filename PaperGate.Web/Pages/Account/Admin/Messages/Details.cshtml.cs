using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Utilities.Helpers;

namespace PaperGate.Web.Pages.Account.Admin.Messages
{
    public class DetailsModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public MessageInfo Message { get; set; }
        public async Task<IActionResult> OnGet(int Id)
        {
            if (Id <= 0)
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToLocalIndex();
            }
            Message = await _unitOfWork.Message.GetAsync(m => m.Id == Id);
            if (Message is null) 
            {
                ShowError(ErrorMessages.NOTFOUND);
                return RedirectToLocalIndex();
            }
            if(Message.Read is false)
            {
                Message.Read = true;
                await _unitOfWork.Message.UpdateAsync(Message);
                await _unitOfWork.SaveChangesAsync();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteMessage()
        {
            try
            {
                if (Message.Id <= 0)
                {
                    ShowError(ErrorMessages.IDINVALID);
                    return Page(Message);
                }
                await _unitOfWork.Message.RemoveAsync(Message);
                await _unitOfWork.SaveChangesAsync();
                ShowSuccess();
                return RedirectToLocalIndex();
            }
            catch
            {
                ShowError(ErrorMessages.ERRORHAPPEDNED);
                return Page();
            }

        }
    }
}
