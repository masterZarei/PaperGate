using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Infra.Data;

namespace PaperGate.Web.Pages.Account.Admin.Keywords
{
    public class DeleteModel : PageModel
    {
        private readonly PaperGate.Infra.Data.AppDbContext _context;

        public DeleteModel(PaperGate.Infra.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public KeywordInfo KeywordInfo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keywordinfo = await _context.Keywords.FirstOrDefaultAsync(m => m.Id == id);

            if (keywordinfo is not null)
            {
                KeywordInfo = keywordinfo;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keywordinfo = await _context.Keywords.FindAsync(id);
            if (keywordinfo != null)
            {
                KeywordInfo = keywordinfo;
                _context.Keywords.Remove(KeywordInfo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
