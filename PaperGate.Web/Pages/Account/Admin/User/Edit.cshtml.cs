using AutoMapper;
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
public class EditModel : MyPageModel
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public EditModel(IUserService userService, IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    [BindProperty]
    public User_ED_Dto EditDto { get; set; }
    public async Task<IActionResult> OnGet(string? Id)
    {
        try
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToSpecialPage(StaticPages.Login);
            }
            if (string.IsNullOrWhiteSpace(Id))
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToIndex();
            }
            var user = await _userService.GetUserById(Id);
            var userResult = user;

            if (user is null)
            {
                ShowError(customMessage: "کاربر یافت نشد!");
                return RedirectToIndex();
            }
            EditDto = _mapper.Map<User_ED_Dto>(userResult);
            await InitList(EditDto.Id);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin Edit User OnGet Failed!", EditDto);
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return RedirectToIndex();
        }

    }
    private async Task InitList(string? userId = null)
    {
        if (string.IsNullOrEmpty(userId))
        {
            var currentUser = await _userService.GetUserByUsername(phoneNumber: User?.Identity?.Name);
            if (currentUser is null)
            {
                ShowError();
            }
            userId = currentUser.Id;
        }

        string? role = await _userService.GetUsersRole(userId);

        EditDto.RolesList = new SelectList(Roles.GetRoles, "Value", "Value", role);
    }
    public async Task<IActionResult> OnPost()
    {
        try
        {

            #region Validation
            UserEditDtoValidation validator = new();
            var validation = validator.Validate(EditDto);
            if (validation.IsValid is false || ModelState.IsValid is false)
            {
                validation.AddToModelState(ModelState);
                string error = string.Empty;
                foreach (var item in validation.Errors)
                {
                    error = string.Join("\t", item.ErrorMessage.Replace("'", string.Empty));
                }
                ShowError(ErrorMessages.CUSTOM, error);
                return RedirectToPage("Edit", new { EditDto.Id });
            }
            #endregion

            var result = await _userService.UpdateUser(EditDto);
            if (result.Succeeded is false)
            {
                foreach (var error in result.Errors)
                {
                    ShowError(ErrorMessages.CUSTOM, customMessage: error);
                }
                return RedirectToPage("Edit", new { EditDto.Id });
            }
            ShowSuccess();
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin Edit User OnPost Failed!", EditDto);
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return RedirectToIndex();
        }
    }
}