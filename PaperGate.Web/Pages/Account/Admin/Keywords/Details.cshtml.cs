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
    public class DetailsModel : PageModel
    {
        private readonly PaperGate.Infra.Data.AppDbContext _context;

        public DetailsModel(PaperGate.Infra.Data.AppDbContext context)
        {
            _context = context;
        }

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
    }
}
