using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Core.Libraries.Generators;
using PaperGate.Infra.Data;
using PaperGate.Web.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.Utilities.Libraries;
using PaperGate.Web.ViewModels;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.Papers
{
    public class CreateModel : MyPageModel
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IFileManagementService _fileManagementService;
        private readonly IUserService _userService;
        private readonly IHTMLToolsService _hTMLToolsService;

        public CreateModel(AppDbContext context,
            IMapper mapper,
            ILogger logger,
            IFileManagementService fileManagementService,
            IUserService userService,
            IHTMLToolsService hTMLToolsService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _fileManagementService = fileManagementService;
            _userService = userService;
            _hTMLToolsService = hTMLToolsService;
        }
        [BindProperty]
        public PaperCreateDto PaperDto { get; set; }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                string username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToSpecialPage(StaticPages.Login);
                }
                PaperDto = new()
                {
                    /*MultipleFilesUp = []*/
                };


                return Page();
            }
            catch (Exception ex)
            {

                ShowError(ErrorMessages.ERRORHAPPEDNED);
                _logger.Fatal(ex, ex.Message, "paper Create Failed On OnGet", PaperDto);
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
                #endregion

                //Upload paper photo
                #region Uploading Files
                #region Pictures
                if (PaperDto.FileUpload is null)
                {
                    ShowError(ErrorMessages.CUSTOM, customMessage: "لطفا حداقل یک عکس برای بلاگ انتخاب کنید");
                    return RedirectToPage("Create");
                }
                var uploadResult = await _fileManagementService.Upload(new FMServiceUploadViewModel
                {
                    Files = [PaperDto?.FileUpload],
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
                    return RedirectToPage("Create", new { PaperDto });
                }
                PaperDto.Picture = uploadResult.Result as string;

                #endregion

                #endregion//https://localhost:7096/Account/Admin/Papers

                //If everything was OK
                var user = await _userService.GetUserByUsername(User?.Identity?.Name);
                if (user == null)
                {
                    ShowError(ErrorMessages.CUSTOM, "دسترسی نامعتبر");
                    return RedirectToIndex();
                }
                PostInfo paper = _mapper.Map<PostInfo>(PaperDto);
                paper.AuthorId = user.Id;
                paper.Slug = SlugGenerator.GenerateSlug(paper.Title, [.. _context.Papers.AsNoTracking()]);
                paper.Summary = _hTMLToolsService.SanitizeContent(paper.Summary);
                paper.Content = _hTMLToolsService.SanitizeContent(paper.Content);
                await _context.AddAsync(paper);
                await _context.SaveChangesAsync();
                ShowSuccess();
                var Addedpaper = await _context.Papers.FirstOrDefaultAsync(p => p.Title == paper.Title);
                ShowInfo("درصورت نیاز دسته بندی های لازم را برای پست انتخاب کنید");
                return RedirectToPage("Edit", new { Addedpaper.Id });
            }
            catch (Exception ex)
            {

                _logger.Fatal(ex, ex.Message, "ایجاد پست با خطا مواجه شد", PaperDto);

                ShowError(ErrorMessages.CUSTOM, customMessage: "در فرآیند اضافه کردن پست خطایی رخ داد. لطفا بعدا امتحان کنید");
                return RedirectToPage("./Index");
            }

        }
    }
}
