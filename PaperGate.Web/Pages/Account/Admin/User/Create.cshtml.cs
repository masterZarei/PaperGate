using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaperGate.Core.DTOs;
using PaperGate.Core.Interfaces;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Core.Libraries.StaticValues;
using PaperGate.Core.Validations.UserDtosValidator;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.User;
public class CreateModel : MyPageModel
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public CreateModel(IUserService userService, IUnitOfWork unitOfWork, ILogger logger)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    [BindProperty]
    public UserCreateDto CreateDto { get; set; }
    public IActionResult OnGet()
    {
        try
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToSpecialPage(StaticPages.Login);
            }
            InitList();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin User Create Failed On OnGet");
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return RedirectToIndex();
        }

    }
    private void InitList()
    {
        CreateDto = new UserCreateDto()
        {
            RolesList = new SelectList(Roles.GetRoles, "Value", "Value")
        };
    }
    public async Task<IActionResult> OnPost()
    {
        try
        {
            #region Validation
            UserCreateDtoValidation validator = new();
            var validation = validator.Validate(CreateDto);
            if (validation.IsValid is false || ModelState.IsValid is false)
            {
                validation.AddToModelState(ModelState);
                InitList();
                foreach (var err in validation.Errors)
                {
                    ShowError(ErrorMessages.CUSTOM, err.ErrorMessage);
                }
                return Page();
            }

            #endregion
            var result = await _userService.CreateUser(CreateDto);
            if (result.Succeeded is false)
            {
                foreach (var error in result.Errors)
                {
                    ShowError(ErrorMessages.CUSTOM, customMessage: error);
                }
                InitList();
                return Page();
            }
            ShowSuccess();
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin User Create Failed On OnPost");
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return RedirectToIndex();
        }

    }
}