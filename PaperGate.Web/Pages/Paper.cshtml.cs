using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Interfaces;
using PaperGate.Web.ViewModels;

namespace PaperGate.Web.Pages
{
    public class PaperModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaperModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public PaperDetailsDto PaperDto { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAddCmt()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Login");
            }

            var userId = await _unitOfWork.Post.GetAsync(u => u.Author.UserName == User.Identity.Name, q=>q.Include(u=>u.Author));

            if (userId is null)
            {
                return RedirectToPage("~/");
            }

            return RedirectToPage();

        }
    }
}
