using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PaperGate.Core.Interfaces;
using System.Threading.Tasks;

namespace PaperGate.Web.Pages.Account.Admin.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Temp

        public async Task OnGet()
        {
            var Cats = await _unitOfWork.Category.GetAllAsync();
        }
    }
}
