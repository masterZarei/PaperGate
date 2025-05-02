using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities.Categories;
using PaperGate.Infra.Data;

namespace PaperGate.Web.Pages.Account.Admin.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly PaperGate.Infra.Data.AppDbContext _context;

        public DeleteModel(PaperGate.Infra.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CategoryInfo CategoryInfo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryinfo = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (categoryinfo is not null)
            {
                CategoryInfo = categoryinfo;

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

            var categoryinfo = await _context.Categories.FindAsync(id);
            if (categoryinfo != null)
            {
                CategoryInfo = categoryinfo;
                _context.Categories.Remove(CategoryInfo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
