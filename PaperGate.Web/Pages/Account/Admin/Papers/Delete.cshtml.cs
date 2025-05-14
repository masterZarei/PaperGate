using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces.Repositories;
using PaperGate.Web.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.Utilities.Libraries;
using PaperGate.Web.ViewModels;
using ILogger = Serilog.ILogger;
namespace PaperGate.Web.Pages.Account.Admin.Papers
{
    public class DeleteModel : MyPageModel
    {
        private readonly IGenericRepository<PaperInfo> _context;
        private readonly IMapper _mapper;
        private readonly IFileManagementService _fileManagementService;
        private readonly ILogger _logger;

        public DeleteModel(IGenericRepository<PaperInfo> context, IMapper mapper, IFileManagementService fileManagementService, ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _fileManagementService = fileManagementService;
            _logger = logger;
        }

        [BindProperty]
        public PaperDeleteDto PaperDto { get; set; }

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
                var Paper = await _context.GetAsync(p => p.Id == Id);
                if (Paper is null)
                {
                    ShowError(ErrorMessages.NOTFOUND);
                    return RedirectToIndex();
                }
                PaperDto = _mapper.Map<PaperDeleteDto>(Paper);

                return Page();
            }
            catch (Exception ex)
            {

                ShowError(ErrorMessages.ERRORHAPPEDNED);
                _logger.Fatal(ex, "Delete Create Failed On OnGet", PaperDto);
                return RedirectToPage("./Index");
            }

        }

        public async Task<IActionResult> OnPostAsync(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    ShowError(ErrorMessages.IDINVALID);
                    return RedirectToIndex();
                }
                var Paper = await _context.GetAsync(p => p.Id == Id);
                if (Paper is null)
                {
                    ShowError(ErrorMessages.NOTFOUND);
                    return RedirectToIndex();
                }
                #region Delete Images
                var deleteImagesResult = await _fileManagementService.Delete(Paper.Picture, StaticValues.PaperImagesPath);
                if (deleteImagesResult.Succeeded is false)
                {
                    ShowError(customMessage: "در فرآیند حذف فایل های پست خطایی پیش آمد");
                }
                Paper.Picture = string.Empty;
                #endregion


                await _context.RemoveAsync(Paper);
                var result = await _context.SaveChangesAsync();
                if (result.Succeeded is false)
                {
                    ShowError(ErrorMessages.CUSTOM, customMessage: "در فرآیند حذف پست خطایی رخ داد. لطفا بعدا امتحان کنید");
                    return Page();
                }
                ShowSuccess();
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {

                ShowError(ErrorMessages.ERRORHAPPEDNED);
                _logger.Fatal(ex, "Paper Delete Failed On OnPost", PaperDto);
                return RedirectToPage("./Index");
            }

        }
    }
}
