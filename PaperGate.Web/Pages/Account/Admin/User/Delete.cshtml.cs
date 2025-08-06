using Ardalis.GuardClauses;
using AutoMapper;
using PaperGate.Core.DTOs;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.User;
public class DeleteModel : MyPageModel
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public DeleteModel(IUserService userService, IMapper mapper, ILogger logger)
    {
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
    }
    [BindProperty]
    public UserDeleteDto DeleteDto { get; set; }
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
            if (user is null)
            {
                ShowError(ErrorMessages.NOTFOUND);
                return RedirectToIndex();
            }
            DeleteDto = _mapper.Map<UserDeleteDto>(user);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin User Delete Failed On OnGet");
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return RedirectToIndex();
        }

    }
    public async Task<IActionResult> OnPost(string Id)
    {
        try
        {
            #region Validation
            Guard.Against.Null(DeleteDto);
            if (string.IsNullOrWhiteSpace(Id) || Id != DeleteDto.Id)
            {
                ShowError(ErrorMessages.ERRORHAPPEDNED);
                return RedirectToIndex();
            }
            #endregion
            var result = await _userService.SoftRemoveUser(DeleteDto);
            if (result.Succeeded is false)
            {
                ShowError(customMessage: "در فرآیند حذف کاربر خطایی رخ داد");
                return Page();
            }
            ShowSuccess();
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin User Delete Failed On OnPost");
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return RedirectToIndex();
        }

    }
}
