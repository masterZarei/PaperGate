using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.DTOs;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Pages.Account.Admin.User;
public class IndexModel : MyPageModel
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public IndexModel(IUserService userService, IMapper mapper, ILogger logger)
    {
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
    }
    public IReadOnlyList<UserListDto> UserListDto { get; set; }
    public async Task<IActionResult> OnGet(string searchName, string searchNumber)
    {
        try
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToSpecialPage(StaticPages.Login);
            }

            var UsersList = await _userService.GetAllUsers(searchName, searchNumber);

            UserListDto = _mapper.Map<IReadOnlyList<UserListDto>>(UsersList);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin User Index Failed On OnGet");
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return RedirectToIndex();
        }

    }
    //@(userManager.GetRolesAsync(new UserInfo() { Id = user.Id }).Result[0].ToString().FromBase64())
    public async Task<string> GetRolesAsync(string userId)
    {
        try
        {
            string role = await _userService.GetUsersRole(userId);
            if (role is null)
                return "Invalid Base64";

            return role;
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin User Index Failed");
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return "خطا";
        }

    }

}