using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperGate.Infra.Data;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.ViewModels;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.Papers
{
    public class IndexModel : MyPageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public IndexModel(AppDbContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public IReadOnlyList<PaperListDto> PaperDto { get; set; }
        public async Task<IActionResult> OnGet(string? searchName)
        {
            try
            {
                string username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToSpecialPage(StaticPages.Login);
                }
                if (!string.IsNullOrEmpty(searchName))
                {
                    var FilteredPaperList = await _context.Papers
                        .Where(a => a.Title.Contains(searchName.Trim())).ToListAsync();

                    PaperDto = _mapper.Map<List<PaperListDto>>(FilteredPaperList.OrderByDescending(p => p.CreatedOn));
                    return Page();
                }

                var PaperList = await _context.Papers
                    .ToListAsync();
                PaperDto = _mapper.Map<List<PaperListDto>>(PaperList.OrderByDescending(p => p.CreatedOn));

                return Page();
            }
            catch (Exception ex)
            {

                ShowError(ErrorMessages.ERRORHAPPEDNED);
                _logger.Fatal(ex, "Index Paper Failed On OnGet");
                return RedirectToPage("./Index");
            }
        }
    }
}
