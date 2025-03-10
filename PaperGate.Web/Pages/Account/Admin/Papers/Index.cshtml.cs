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
    public class IndexModel : PageModel
    {
        private readonly PaperGate.Infra.Data.AppDbContext _context;

        public IndexModel(PaperGate.Infra.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<PaperInfo> PaperInfo { get;set; } = default!;

        public async Task OnGetAsync()
        {
            PaperInfo = await _context.Papers
                .Include(p => p.Author).ToListAsync();
        }
    }
}
