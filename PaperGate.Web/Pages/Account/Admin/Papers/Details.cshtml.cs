using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities;
using PaperGate.Infra.Data;

namespace PaperGate.Web.Pages.Account.Admin.Papers
{
    public class DetailsModel : PageModel
    {
        private readonly PaperGate.Infra.Data.AppDbContext _context;

        public DetailsModel(PaperGate.Infra.Data.AppDbContext context)
        {
            _context = context;
        }

        public PaperInfo PaperInfo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paperinfo = await _context.Papers.FirstOrDefaultAsync(m => m.Id == id);

            if (paperinfo is not null)
            {
                PaperInfo = paperinfo;

                return Page();
            }

            return NotFound();
        }
    }
}
