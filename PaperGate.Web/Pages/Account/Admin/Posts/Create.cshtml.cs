using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Core.Libraries.Generators;
using PaperGate.Web.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.Utilities.Libraries;
using PaperGate.Web.ViewModels;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.Posts
{
    public class CreateModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IFileManagementService _fileManagementService;
        private readonly IUserService _userService;
        private readonly IHTMLToolsService _hTMLToolsService;

        public CreateModel(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger logger,
            IFileManagementService fileManagementService,
            IUserService userService,
            IHTMLToolsService hTMLToolsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _fileManagementService = fileManagementService;
            _userService = userService;
            _hTMLToolsService = hTMLToolsService;
        }
        [BindProperty]
        public PostCreateDto PostDto { get; set; }
        [BindProperty]
        public int Sub { get; set; }
        public async Task<IActionResult> OnGet(int sub)
        {
            try
            {
                string username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToSpecialPage(StaticPages.Login);
                }
                PostDto = new()
                {
                    /*MultipleFilesUp = []*/
                };
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
                Sub = sub;
                return Page();
            }
            catch (Exception ex)
            {

                ShowError(ErrorMessages.ERRORHAPPEDNED);
                _logger.Fatal(ex, ex.Message, "paper Create Failed On OnGet", PostDto);
                return RedirectToPage("./Index");
            }

        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    ShowError(ErrorMessages.CUSTOM, "لطفا فیلد های ضروری را پر کنید");
                    return RedirectToPage("Create");
                }
                if (Sub <= 0)
                {
                    ShowWarning("دسته بندی مورد نظر نامعتبر است");
                    return RedirectToLocalIndex();
                }
                var category = await _unitOfWork.Category.GetAsync(c => c.Id == Sub);
                if (category is null)
                {
                    ShowWarning("دسته بندی مورد نظر نامعتبر است");
                    return RedirectToLocalIndex();
                }
                #endregion

                //Upload paper photo
                #region Uploading Files
                #region Pictures
                if (PostDto.FileUpload is null)
                {
                    ShowError(ErrorMessages.CUSTOM, customMessage: "لطفا حداقل یک عکس برای بلاگ انتخاب کنید");
                    return RedirectToPage("Create");
                }
                var uploadResult = await _fileManagementService.Upload(new FMServiceUploadViewModel
                {
                    Files = [PostDto?.FileUpload],
                    FileType = FileType.Image,
                    FolderPath = StaticValues.PaperImagesPath,
                    FileCount = FileCount.Single
                });
                if (uploadResult.Succeeded is false)
                {
                    foreach (var error in uploadResult.Errors)
                    {
                        ShowError(ErrorMessages.CUSTOM, customMessage: error);
                    }
                    return RedirectToPage("Create", new { PostDto });
                }
                PostDto.Picture = uploadResult.Result as string;

                #endregion

                #endregion//https://localhost:7096/Account/Admin/Papers

                //If everything was OK
                var user = await _userService.GetUserByUsername(User?.Identity?.Name);
                if (user == null)
                {
                    ShowError(ErrorMessages.CUSTOM, "دسترسی نامعتبر");
                    return RedirectToIndex();
                }
                PostInfo post = _mapper.Map<PostInfo>(PostDto);
                post.CategoryId = Sub;
                post.AuthorId = user.Id;
                post.Slug = SlugGenerator.GenerateSlug(post.Title, [.. await _unitOfWork.Post.GetAllReadOnlyAsync()]);
                await _unitOfWork.Post.AddAsync(post);
                await _unitOfWork.SaveChangesAsync();
                ShowSuccess();
                var Addedpaper = await _unitOfWork.Post.GetAsync(p => p.Title == post.Title);
                ShowInfo("درصورت نیاز دسته بندی های لازم را برای پست انتخاب کنید");
                return RedirectToPage("Edit", new { Addedpaper.Id });
            }
            catch (Exception ex)
            {

                _logger.Fatal(ex, ex.Message, "ایجاد پست با خطا مواجه شد", PostDto);

                ShowError(ErrorMessages.CUSTOM, customMessage: "در فرآیند اضافه کردن پست خطایی رخ داد. لطفا بعدا امتحان کنید");
                return RedirectToPage("./Index");
            }

        }
    }
}
