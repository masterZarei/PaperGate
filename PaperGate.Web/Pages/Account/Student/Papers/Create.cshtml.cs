using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities;
using PaperGate.Infra.Data;

namespace PaperGate.Web.Pages.Account.Student.Papers
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PaperInfo PaperInfo { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user is null)
            {
                return RedirectToPage("~/");
            }
            PaperInfo.AuthorId = user.Id;
            _context.Papers.Add(PaperInfo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
