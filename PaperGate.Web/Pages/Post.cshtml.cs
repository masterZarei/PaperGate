using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.DTOs;
using PaperGate.Core.Interfaces;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages
{
    public class PostModel : MyPageModel
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public PostModel(
            IMapper mapper,
            ILogger logger,
            IUnitOfWork unitOfWork,
            IUserService userService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        [BindProperty]
        public PublicPostDetailsDto PostDto { get; set; }
        public async Task<IActionResult> OnGet(string? slug)
        {
            if (string.IsNullOrEmpty(slug))
                return RedirectToIndex();

            var post = await _unitOfWork.Post.GetPostBySlugAsync(slug);
            if (post is null)
                return RedirectToIndex();

            PostDto = _mapper.Map<PublicPostDetailsDto>(post);

            //PostDto.CurrentLanguage = lang;
            //lang = TempData.ContainsKey("Lang") ? (Language)TempData["Lang"] : Language.Persian;
            if (TempData.ContainsKey("Lang") == false)
            {
                TempData["Lang"] = Language.Persian;
            }

            PostDto.PostKeywords = post?.Keywords?.ToList();
            PostDto.LatestPosts = [.. (await _unitOfWork.Post.GetAllAsync(p => p.IsActive && p.Id != post.Id))
                             .OrderByDescending(b => b.CreatedOn)
                             .Take(5)];
            PostDto.Author = await _userService.GetUserById(post.AuthorId);

            return Page();
        }
        public async Task<IActionResult> OnPostChangeLanguage()
        {
            try
            {
                if ((Language)TempData["Lang"] == Language.Persian)
                    TempData["Lang"] = Language.English;
                else
                    TempData["Lang"] = Language.Persian;

               // return RedirectToPage("/Post", new { slug = PostDto.Slug });
               // return RedirectToPage("Post", new { PostDto?.Slug });
                return RedirectToPage("/Post", new { slug = PostDto.Slug });

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
