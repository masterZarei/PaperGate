using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities.Categories;
using PaperGate.Infra.Data;

namespace PaperGate.Web.Pages.Account.Admin.Categories
{
    public class EditModel : PageModel
    {
        private readonly PaperGate.Infra.Data.AppDbContext _context;

        public EditModel(PaperGate.Infra.Data.AppDbContext context)
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

            var categoryinfo =  await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (categoryinfo == null)
            {
                return NotFound();
            }
            CategoryInfo = categoryinfo;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CategoryInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryInfoExists(CategoryInfo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CategoryInfoExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
