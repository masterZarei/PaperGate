using PaperGate.Core.DTOs;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;
namespace PaperGate.Web.Pages.Account.Admin.User;
public class ChangePasswordModel : MyPageModel
{
    private readonly IUserService _userService;
    private readonly ILogger _logger;

    public ChangePasswordModel(IUserService userService, ILogger logger)
    {
        _userService = userService;
        _logger = logger;
    }
    [BindProperty]
    public AdminChangePasswordDto PasswordDto { get; set; }
    public async Task<IActionResult> OnGet(string Id)
    {
        try
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToSpecialPage(StaticPages.Login);
            }
            if (string.IsNullOrEmpty(Id))
            {
                ShowError(ErrorMessages.IDINVALID);
                return RedirectToIndex();
            }
            var user = await _userService.GetUserById(Id);
            if (user is null)
            {
                ShowError(ErrorMessages.NOTFOUND);
                return RedirectToIndex();
            }
            PasswordDto = new AdminChangePasswordDto
            {
                Id = Id,
                FullName = $"{user.Name} {user.LastName}",
                PasswordChanger = PasswordChanger.Admin
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin User ChangePassword Failed On OnGet");
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return RedirectToIndex();
        }

    }
    public async Task<IActionResult> OnPost()
    {
        try
        {
            #region Validation
            if (!ModelState.IsValid)
            {
                return Page();
            }
            #endregion

            var result = await _userService.ChangeUsersPassword(PasswordDto);
            if (result.Succeeded is false)
            {
                foreach (var error in result.Errors)
                {
                    //ShowError(error);
                    ModelState.AddModelError(string.Empty, error);
                }
                return Page();
            }
            ShowSuccess();
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin User ChangePassword Failed On OnPost");
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return RedirectToIndex();
        }

    }
}
