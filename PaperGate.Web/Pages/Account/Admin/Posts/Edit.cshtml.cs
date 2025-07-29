using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Infra.Data;
using PaperGate.Web.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using PaperGate.Web.Utilities.Libraries;
using PaperGate.Web.ViewModels;
using ILogger = Serilog.ILogger;
namespace PaperGate.Web.Pages.Account.Admin.Posts
{
    public class EditModel : MyPageModel
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IFileManagementService _fileManagementService;
        private readonly IHTMLToolsService _hTMLToolsService;

        public EditModel(AppDbContext context,
            IMapper mapper,
            ILogger logger,
            IFileManagementService fileManagementService,
            IHTMLToolsService hTMLToolsService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _fileManagementService = fileManagementService;
            _hTMLToolsService = hTMLToolsService;
        }
        [BindProperty]
        public PaperEditDto PaperDto { get; set; }

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
                var Paper = await _context.Papers.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id);
                if (Paper is null)
                {
                    ShowError(ErrorMessages.NOTFOUND);
                    return RedirectToIndex();
                }
                PaperDto = _mapper.Map<PaperEditDto>(Paper);

                InitLists();
                return Page();
            }
            catch (Exception ex)
            {

                ShowError(ErrorMessages.ERRORHAPPEDNED);
                _logger.Fatal(ex, "Edit Paper Failed On OnGet", PaperDto);
                return RedirectToPage("./Index");
            }

        }
        private void InitLists()
        {

            #region Category

            /*  PaperDto.PaperCategories = (from categories in _context.Categories.ToList()
                                          join categoryToPapers in _context.PaperCategories.ToList()
                                          on categories.Id equals categoryToPapers.CategoryId
                                          where categoryToPapers.PaperId == PaperDto.Id
                                          select categories).ToList();*/

            /*            PaperDto.AvailableCategories = (from categories in _context.Categories.ToList()
                                                        where !PaperDto.PaperCategories.Contains(categories)
                                                        select categories).ToList();
                        if (PaperDto.AvailableCategories is not null)
                            PaperDto.CategoryList = new SelectList(PaperDto.AvailableCategories, nameof(CategoryInfo.Id), nameof(CategoryInfo.Title));*/
            #endregion

            #region Keyword

            PaperDto.PaperKeywords = (from keywords in _context.Keywords.ToList()
                                      join keywordToPapers in _context.PaperKeywords.ToList()
                                      on keywords.Id equals keywordToPapers.KeywordId
                                      where keywordToPapers.PaperId == PaperDto.Id
                                      select keywords).ToList();

            PaperDto.AvailableKeywords = (from keyword in _context.Keywords.ToList()
                                          where !PaperDto.PaperKeywords.Contains(keyword)
                                          select keyword).ToList();
            if (PaperDto.AvailableKeywords is not null)
                PaperDto.KeywordList = new SelectList(PaperDto.AvailableKeywords, nameof(KeywordInfo.Id), nameof(KeywordInfo.Title));
            #endregion

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPost()
        {
            try
            {
                #region Validation
                if (!ModelState.IsValid)
                {
                    ShowError(ErrorMessages.CUSTOM, "لطفا فیلد های ضروری را پر کنید");
                    InitLists();
                    return RedirectToPage("Edit", new { PaperDto.Id });
                }
                #endregion
                //Uploading Files
                #region Image
                //If User has selected one or more images
                if (PaperDto?.FileUpload is not null)
                {
                    var uploadResult = await _fileManagementService.Alter(new FMServiceAlterViewModel
                    {
                        Files = [PaperDto?.FileUpload],
                        LastFilesNames = PaperDto?.Picture,
                        FileType = FileType.Image,
                        FolderPath = StaticValues.PaperImagesPath,
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
                    PaperDto.Picture = uploadResult.Result as string;
                }



                #endregion



                PostInfo post = _mapper.Map<PostInfo>(PaperDto);
                post.Summary = _hTMLToolsService.SanitizeContent(post.Summary);
                post.Content = _hTMLToolsService.SanitizeContent(post.Content);
                _context.Papers.Update(post);
                await _context.SaveChangesAsync();
                ShowSuccess();
                return RedirectToPage("./Index", new { sub = post.CategoryId });
            }
            catch (Exception ex)
            {
                ShowError(ErrorMessages.CUSTOM, customMessage: "در فرآیند ویرایش مقاله خطایی رخ داد. لطفا بعدا امتحان کنید");
                _logger.Fatal(ex, ex.Message, "ویرایش مقاله از مقاله با خطا مواجه شد");
                return Page();
            }



        }
        #region Category PageHandlers
        /*   public async Task<IActionResult> OnPostAddCategory()
           {
               #region Validation
               if (string.IsNullOrEmpty(PaperDto.SelectedCategory))
               {
                   ShowError(ErrorMessages.IDINVALID);
                   return RedirectToPage("Edit", new { PaperDto?.Id });
               }
               bool isValid = int.TryParse(PaperDto.SelectedCategory, out int Id);
               if (isValid is false)
               {
                   ShowError(ErrorMessages.IDINVALID);
                   return RedirectToPage("Edit", new { PaperDto?.Id });
               }
               if (!await _context.Categories.AnyAsync(a => a.Id == Id))
               {
                   ShowError(ErrorMessages.NOTFOUND);
                   return RedirectToPage("Edit", new { PaperDto?.Id });
               }
               //Is already added
               if (await _context.PaperCategories.AnyAsync(a => a.PaperId == PaperDto.Id && a.CategoryId == Id))
               {
                   ShowError(ErrorMessages.CUSTOM, customMessage: "دسته بندی با همین نام برای این مقاله موجود است");
                   return RedirectToPage("Edit", new { PaperDto?.Id });
               }
               #endregion
               try
               {
                   await _context.PaperCategories.AddAsync(new PaperCategoryInfo
                   {
                       CategoryId = Id,
                       PaperId = PaperDto.Id
                   });
                   await _context.SaveChangesAsync();
               }
               catch (Exception ex)
               {

                   ShowError(ErrorMessages.CUSTOM, customMessage: "اضافه کردن دسته بندی به مقاله با خطا مواجه شد");
                   _logger.Fatal(ex, ex.Message, "اضافه کردن دسته بندی به مقاله با خطا مواجه شد");
                   return RedirectToPage("Edit", new { PaperDto?.Id });
               }
               ShowSuccess();
               InitLists();
               return RedirectToPage("Edit", new { PaperDto?.Id });
           }
           public async Task<IActionResult> OnPostRemoveCategory(int Id)
           {
               if (Id == 0 || PaperDto?.Id == 0)
               {
                   ShowError(ErrorMessages.IDINVALID);
                   return RedirectToPage("Edit", new { PaperDto?.Id });
               }
               var category = await _context.PaperCategories
                   .FirstOrDefaultAsync(a => a.CategoryId == Id && a.PaperId == PaperDto.Id);

               if (category is null)
               {
                   ShowError(ErrorMessages.NOTFOUND);
                   return RedirectToPage("Edit", new { PaperDto?.Id });
               }
               try
               {
                   _context.PaperCategories.Remove(category);
                   await _context.SaveChangesAsync();
               }
               catch (Exception ex)
               {
                   ShowError(ErrorMessages.CUSTOM, customMessage: "حذف کردن دسته بندی از مقاله با خطا مواجه شد");
                   _logger.Fatal(ex, ex.Message, "حذف کردن دسته بندی از مقاله با خطا مواجه شد");
                   return RedirectToPage("Edit", new { PaperDto?.Id });
               }

               ShowSuccess();
               InitLists();
               return RedirectToPage("Edit", new { PaperDto?.Id });
           }*/
        #endregion


        #region Keyword PageHandlers
        public async Task<IActionResult> OnPostAddKeyword()
        {
            #region Validation
            if (string.IsNullOrEmpty(PaperDto.SelectedKeyword))
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToPage("Edit", new { PaperDto?.Id });
            }
            bool isValid = int.TryParse(PaperDto.SelectedKeyword, out int Id);
            if (isValid is false)
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToPage("Edit", new { PaperDto?.Id });
            }
            if (!await _context.Keywords.AnyAsync(a => a.Id == Id))
            {
                ShowError(ErrorMessages.NOTFOUND);
                return RedirectToPage("Edit", new { PaperDto?.Id });
            }
            //Is already added
            if (await _context.PaperKeywords.AnyAsync(a => a.PaperId == PaperDto.Id && a.KeywordId == Id))
            {
                ShowError(ErrorMessages.CUSTOM, customMessage: "کلمه کلیدی با همین نام برای این مقاله موجود است");
                return RedirectToPage("Edit", new { PaperDto?.Id });
            }
            #endregion
            try
            {
                await _context.PaperKeywords.AddAsync(new PaperKeywordInfo
                {
                    KeywordId = Id,
                    PaperId = PaperDto.Id
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                ShowError(ErrorMessages.CUSTOM, customMessage: "اضافه کردن کلمه کلیدی به مقاله با خطا مواجه شد");
                _logger.Fatal(ex, ex.Message, "اضافه کردن کلمه کلیدی به مقاله با خطا مواجه شد");
                return RedirectToPage("Edit", new { PaperDto?.Id });
            }
            ShowSuccess();
            InitLists();
            return RedirectToPage("Edit", new { PaperDto?.Id });
        }
        public async Task<IActionResult> OnPostRemoveKeyword(int Id)
        {
            if (Id == 0 || PaperDto?.Id == 0)
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToPage("Edit", new { PaperDto?.Id });
            }
            var Keyword = await _context.PaperKeywords
                .FirstOrDefaultAsync(a => a.KeywordId == Id && a.PaperId == PaperDto.Id);

            if (Keyword is null)
            {
                ShowError(ErrorMessages.NOTFOUND);
                return RedirectToPage("Edit", new { PaperDto?.Id });
            }
            try
            {
                _context.PaperKeywords.Remove(Keyword);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ShowError(ErrorMessages.CUSTOM, customMessage: "حذف کردن کلمه کلیدی از مقاله با خطا مواجه شد");
                _logger.Fatal(ex, ex.Message, "حذف کردن لمه کلیدی از مقاله با خطا مواجه شد");
                return RedirectToPage("Edit", new { PaperDto?.Id });
            }

            ShowSuccess();
            InitLists();
            return RedirectToPage("Edit", new { PaperDto?.Id });
        }
        #endregion
    }
}
