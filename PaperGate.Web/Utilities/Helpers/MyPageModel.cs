using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PaperGate.Web.Utilities.Libraries;
using System.Security.Claims;

namespace PaperGate.Web.Utilities.Helpers;

public class MyPageModel : PageModel
{
    public const string AdminPanelPath = "/Account/Admin";


    private const string _error = "danger";
    private const string _info = "info";
    private const string _warning = "warning";
    private const string _success = "success";

    #region Notifications
    public enum ErrorMessages
    {
        NOTFOUND,
        ERRORHAPPEDNED,
        IDINVALID,
        FILLREQUESTEDDATA,
        CUSTOM
    }
    public void ShowError(ErrorMessages errorMessage = ErrorMessages.ERRORHAPPEDNED, string? customMessage = "")
    {
        string message = errorMessage switch
        {
            ErrorMessages.NOTFOUND => "موردی یافت نشد!",
            ErrorMessages.ERRORHAPPEDNED => "خطایی رخ داد!!",
            ErrorMessages.IDINVALID => "شناسه وارد شده نامعتبر است!",
            ErrorMessages.FILLREQUESTEDDATA => "لطفا فیلدهای ضروری را با مقادیر صحیح پر کنید!",
            ErrorMessages.CUSTOM => customMessage,
            _ => customMessage is null ? "خطایی رخ داد!!" : customMessage,
        };
        TempData["Msg"] = message;
        TempData["State"] = _error;
    }
    public void ShowInfo(string message)
    {
        TempData["State"] = _info;
        TempData["Msg"] = message;
    }
    public void ShowWarning(string message)
    {
        TempData["State"] = _warning;
        TempData["Msg"] = message;
    }
    public void ShowSuccess(string message = "با موفقیت انجام شد!")
    {
        TempData["State"] = _success;
        TempData["Msg"] = message;
    }
    #endregion
    #region Security
    public string? GetClaim(string key)
    {
        Guard.Against.NullOrEmpty(key);

        if (User?.Identity?.Name is null || User?.Identity?.IsAuthenticated is false)
        {
            return null;
        }
        return (User?.Identity as ClaimsIdentity)?.Claims?.Where(c => c.Type == key)?.FirstOrDefault()?.Value;
    }
    #endregion
    #region Redirection
    public IActionResult Page(object routeValue)
    {
        return RedirectToPage(routeValues: routeValue);
    }
    public IActionResult RedirectToSpecialPage(StaticPages pages, string ReturnUrl = "")
    {
        /*return pages switch
        {
            StaticPages.Login => RedirectToPage($"{Url.Content("~")}{StaticValues.LoginPath}{ReturnUrl}"),
            StaticPages.Register => RedirectToPage($"{Url.Content("~")}{StaticValues.RegisterPath}{ReturnUrl}"),
            StaticPages.Index => Redirect(Url.Content("~/")),
            _ => RedirectToPage(Url.Content("~/")),
        };*/
        return pages switch
        {
            StaticPages.Login => Redirect($"{Url.Content("~")}{StaticValues.LoginPath}?returnUrl={ ReturnUrl}"),
            StaticPages.Register => Redirect($"{Url.Content("~")}{StaticValues.RegisterPath}?returnUrl={ReturnUrl}"),
            StaticPages.Index => Redirect(Url.Content("~/")),
            _ => Redirect(Url.Content("~/")),
        };
    }
    public IActionResult RedirectToIndex()
    {
        return LocalRedirect(Url.Content("~/"));
    }
    #endregion
    public enum StaticPages
    {
        Login,
        Register,
        Index
    }
}