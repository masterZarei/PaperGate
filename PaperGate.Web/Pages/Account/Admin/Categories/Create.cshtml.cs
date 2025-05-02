using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaperGate.Core.Entities.Categories;
using PaperGate.Infra.Data;

namespace PaperGate.Web.Pages.Account.Admin.Categories
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
        public CategoryInfo CategoryInfo { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Categories.Add(CategoryInfo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
