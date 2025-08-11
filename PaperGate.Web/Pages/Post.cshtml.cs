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
        public PublicPostDetailsDto PostDto { get; set; }
        public async Task<IActionResult> OnGet(string? slug)
        {
            try
            {
                if (string.IsNullOrEmpty(slug))
                    return RedirectToIndex();

                var post = await _unitOfWork.Post.GetPostBySlugAsync(slug);

                if (post is null)
                    return RedirectToIndex();

                PostDto = _mapper.Map<PublicPostDetailsDto>(post);

                PostDto.PostKeywords = post?.Keywords?.ToList();
                PostDto.LatestPosts = [.. (await _unitOfWork.Post.GetAllAsync(p => p.IsActive)).OrderByDescending(b => b.CreatedOn).Take(5)];
                var author = await _userService.GetUserById(post.AuthorId);
                PostDto.Author = author;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "DANGER DANGER DANGER Blog Details Failed to LOAD! GET METHOD");
                ShowError(ErrorMessages.ERRORHAPPEDNED);
                return RedirectToIndex();
            }
        }
    }
}
