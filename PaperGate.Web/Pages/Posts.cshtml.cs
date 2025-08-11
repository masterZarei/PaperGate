using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.DTOs;
using PaperGate.Core.Interfaces;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;
namespace PaperGate.Web.Pages
{
    public class PostsModel : MyPageModel
    {
        private readonly IPublicInfoService _publicInfoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public PostsModel(IPublicInfoService publicInfoService,IUnitOfWork unitOfWork, ILogger logger)
        {
            _publicInfoService = publicInfoService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [BindProperty]
        public AllPostsDto PostsDto { get; set; }

        public string SubTitle { get; set; }
        public async Task<IActionResult> OnGet(int sub, string? PostTitle = null)
        {
            try
            {
                if (sub <= 0)
                {
                    ShowWarning("دسته بندی مورد نظر نامعتبر است");
                    return RedirectToLocalIndex();
                }
                var category = await _unitOfWork.Category.GetAsync(c => c.Id == sub);
                if (category is null)
                {
                    ShowWarning("دسته بندی مورد نظر نامعتبر است");
                    return RedirectToLocalIndex();
                }
                SubTitle = category.Title;
                PostsDto = await _publicInfoService.GetAllPostsInfoAsync(sub);
                if (!string.IsNullOrEmpty(PostTitle) && PostsDto?.Posts?.Count > 0)
                {
                    PostsDto.Posts = PostsDto.Posts.Where(p => p.Title.Contains(PostTitle) && p.IsActive).ToList();
                    PostsDto.PostTitle = PostTitle;
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "DANGER DANGER DANGER AllPosts FAILED ON OnGet", PostsDto);
                ShowError(ErrorMessages.ERRORHAPPEDNED);
                return RedirectToIndex();
            }
        }
    }
}
