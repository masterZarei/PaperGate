using AutoMapper;
using PaperGate.Core.DTOs;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Web.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;
namespace PaperGate.Web.Pages.Account.Admin.User;
public class DetailsModel : MyPageModel
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public DetailsModel(IUserService userService, IMapper mapper, ILogger logger)
    {
        _userService = userService;
        _mapper = mapper;
        _logger = logger;
    }
    public User_ED_Dto DetailsDto { get; set; }
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
            DetailsDto = _mapper.Map<User_ED_Dto>(user);
            DetailsDto.Role = await _userService.GetUsersRole(Id);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.Fatal(ex, "Admin User Details Failed On OnGet");
            ShowError(ErrorMessages.ERRORHAPPEDNED);
            return RedirectToIndex();
        }

    }
}
