using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Interfaces;
using PaperGate.Infra.Data;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.ViewModels;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.Posts
{
    public class IndexModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public IndexModel(IUnitOfWork unitOfWork, ILogger logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public IReadOnlyList<PaperListDto> PaperDto { get; set; }
        public string CategoryTitle { get; set; }
        public async Task<IActionResult> OnGet(int sub, string? searchName)
        {
            try
            {
                string username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToSpecialPage(StaticPages.Login);
                }
                if (sub <= 0)
                {
                    ShowWarning("دسته بندی مورد نظر نامعتبر است");
                    return RedirectToLocalIndex();
                }
                if (!string.IsNullOrEmpty(searchName))
                {
                    var FilteredPaperList = await _unitOfWork.Post
                        .GetAllReadOnlyAsync(a => a.Title.Contains(searchName.Trim()));

                    PaperDto = _mapper.Map<List<PaperListDto>>(FilteredPaperList.OrderByDescending(p => p.CreatedOn));
                    return Page();
                }
                var category = await _unitOfWork.Category.GetAsync(c => c.Id == sub);
                if (category is null)
                {
                    ShowWarning("دسته بندی مورد نظر نامعتبر است");
                    return RedirectToLocalIndex();
                }
                CategoryTitle = category.Title;
                var PaperList = await _unitOfWork.Post
                    .GetAllReadOnlyAsync(p=>p.CategoryId == category.Id/*, q=>q.Include(c=>c.Category)*/);

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
