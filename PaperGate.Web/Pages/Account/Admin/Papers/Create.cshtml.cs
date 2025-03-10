using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaperGate.Core.Entities;
using PaperGate.Infra.Data;

namespace PaperGate.Web.Pages.Account.Admin.Papers
{
    public class CreateModel : PageModel
    {
        private readonly PaperGate.Infra.Data.AppDbContext _context;

        public CreateModel(PaperGate.Infra.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
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

            _context.Papers.Add(PaperInfo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
