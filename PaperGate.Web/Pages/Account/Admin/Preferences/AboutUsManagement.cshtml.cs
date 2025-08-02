using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities.Template;
using PaperGate.Core.Interfaces;
using PaperGate.Web.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.Utilities.Libraries;
using PaperGate.Web.ViewModels;
using Serilog;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.Preferences
{
    public class AboutUsManagementModel : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileManagementService _fileManagementService;
        private readonly ILogger _logger;

        public AboutUsManagementModel(IUnitOfWork unitOfWork, IMapper mapper, IFileManagementService fileManagementService, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileManagementService = fileManagementService;
            _logger = logger;
        }

        [BindProperty]
        public AboutUsEditDto AboutUsDto { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                string username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToSpecialPage(StaticPages.Login);
                }
                var data = await _unitOfWork.AboutUs.GetAsync();
                if (data == null)
                {
                    await _unitOfWork.AboutUs.AddAsync(new AboutUsInfo
                    {
                        Description = "تنظیم نشده",
                        Image = "~/images/empty.jpg"
                    });
                    await _unitOfWork.SaveChangesAsync();
                    data = await _unitOfWork.AboutUs.GetAsync();
                    AboutUsDto = _mapper.Map<AboutUsEditDto>(data);
                    return Page();
                }
                AboutUsDto = _mapper.Map<AboutUsEditDto>(data);

                return Page();
            }
            catch (Exception ex)
            {
                ShowError(ErrorMessages.ERRORHAPPEDNED);
                _logger.Fatal(ex, "Aboutus mgmt Failed on OnGet", AboutUsDto);
                return RedirectToIndex();
            }

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                string username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return RedirectToSpecialPage(StaticPages.Login);
                }
                if (!ModelState.IsValid)
                {
                    ShowError(ErrorMessages.FILLREQUESTEDDATA);
                    return Page();
                }


                #region Upload Image
                if (AboutUsDto?.ImageUpload is not null)
                {
                    var uploadResult = await _fileManagementService.Alter(new FMServiceAlterViewModel
                    {
                        Files = new List<IFormFile> { AboutUsDto?.ImageUpload },
                        LastFilesNames = AboutUsDto?.Image,
                        FileType = FileType.Image,
                        FolderPath = StaticValues.PreferencesImagesPath,
                        FileCount = FileCount.Single
                    });
                    if (uploadResult.Succeeded is false)
                    {
                        foreach (var error in uploadResult.Errors)
                        {
                            ShowError(customMessage: error);
                        }
                        return Page();
                    }
                    AboutUsDto.Image = uploadResult.Result as string;
                }
                /*else
                {
                    ShowError(customMessage: "لطفا یک عکس انتخاب کنید");
                    return Page(new { SliderDto.Id });
                }*/
                #endregion
                var AboutUs = _mapper.Map<AboutUsInfo>(AboutUsDto);

                await _unitOfWork.AboutUs.UpdateAsync(AboutUs);
                await _unitOfWork.SaveChangesAsync();
                ShowSuccess();
                return RedirectToPage("./AboutUsManagement");
            }
            catch (Exception ex)
            {
                ShowError(ErrorMessages.ERRORHAPPEDNED);
                _logger.Fatal(ex, "Aboutus mgmt Failed on OnPost", AboutUsDto);
                return RedirectToIndex();
            }

        }
    }
}
