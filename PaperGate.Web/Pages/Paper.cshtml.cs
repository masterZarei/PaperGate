using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PaperGate.Infra.Data;
using PaperGate.Web.ViewModels;

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

            var userId = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (userId is null)
            {
                return RedirectToPage("~/");
            }

            return RedirectToPage();

        }
    }
}
