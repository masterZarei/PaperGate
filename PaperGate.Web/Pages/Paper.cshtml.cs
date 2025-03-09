using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;
using PaperGate.Infra.Data;

namespace PaperGate.Web.Pages
{
    public class PaperModel : PageModel
    {
        private readonly AppDbContext _context;

        public PaperModel(AppDbContext context)
        {
            _context = context;
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
            if (PaperDto.Comment == null && string.IsNullOrWhiteSpace(PaperDto?.Comment?.Content))
            {
                ModelState.AddModelError(string.Empty, "لطفا محتویات کامنت را وارد کنید");
                return RedirectToPage();
            }

            var userId = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (userId is null)
            {
                return RedirectToPage("~/");
            }


            _context.Comments.Add(new CommentInfo
            {
                Content = PaperDto.Comment.Content,
                UserId = userId.Id,
                IsVerified = true //فعلا
            });
            return RedirectToPage();

        }
    }
}
