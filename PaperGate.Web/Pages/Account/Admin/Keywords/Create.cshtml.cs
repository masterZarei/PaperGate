using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Infra.Data;

namespace PaperGate.Web.Pages.Account.Admin.Keywords
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
            return Page();
        }

        [BindProperty]
        public KeywordInfo KeywordInfo { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Keywords.Add(KeywordInfo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
