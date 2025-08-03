using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperGate.Infra.Data;
using PaperGate.Web.Utilities.Helpers;
using PostGate.Web.ViewModels;
using ILogger = Serilog.ILogger;
namespace PaperGate.Web.Pages.Account.Admin.Posts
{
    public class DetailsModel : MyPageModel
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DetailsModel(AppDbContext context,
            IMapper mapper,
            ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        [BindProperty]
        public PostDetailsDto PostDto { get; set; }

        public async Task<IActionResult> OnGet(int Id)
        {
            try
            {
                string username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToSpecialPage(StaticPages.Login);
                }
                if (Id == 0)
                {
                    ShowError(ErrorMessages.IDINVALID);
                    return RedirectToIndex();
                }
                var Paper = await _context.Posts
                    /*.Include(c => c.Categories)*/
                    .FirstOrDefaultAsync(p => p.Id == Id);
                if (Paper is null)
                {
                    ShowError(ErrorMessages.NOTFOUND);
                    return RedirectToIndex();
                }
                PostDto = _mapper.Map<PostDetailsDto>(Paper);
                /*PaperDto.PaperCategories = await _context.PaperCategories
                    .Include(c => c.Category)
                    .Where(c => c.PaperId == Id).ToListAsync();*/

                PostDto.PostKeywords = await _context.PaperKeywords
                    .Include(c => c.Keyword)
                    .Where(c => c.PostId == Id).ToListAsync();

                return Page();
            }
            catch (Exception ex)
            {
                ShowError(ErrorMessages.ERRORHAPPEDNED);
                _logger.Fatal(ex.ToString(), $"Paper/Details/PaperViewModelJason:{PostDto}");
                return RedirectToIndex();
            }

        }
    }
}
