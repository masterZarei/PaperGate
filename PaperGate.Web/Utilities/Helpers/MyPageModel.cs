using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PaperGate.Web.Utilities.Libraries;
using System.Globalization;

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
        string message = "Msg";
        if(CultureInfo.CurrentUICulture.Name.StartsWith("fa")){
            message = errorMessage switch
            {

                ErrorMessages.NOTFOUND => "موردی یافت نشد!",
                ErrorMessages.ERRORHAPPEDNED => "خطایی رخ داد!!",
                ErrorMessages.IDINVALID => "شناسه وارد شده نامعتبر است!",
                ErrorMessages.FILLREQUESTEDDATA => "لطفا فیلدهای ضروری را با مقادیر صحیح پر کنید!",
                ErrorMessages.CUSTOM => customMessage,
                _ => customMessage is null ? "خطایی رخ داد!!" : customMessage,
            };
        }
        if (CultureInfo.CurrentUICulture.Name.StartsWith("fa"))
        {
            message = errorMessage switch
            {

                ErrorMessages.NOTFOUND => "No Item Found",
                ErrorMessages.ERRORHAPPEDNED => "Something went wrong",
                ErrorMessages.IDINVALID => "Invalid Id",
                ErrorMessages.FILLREQUESTEDDATA => "Fill requested data",
                ErrorMessages.CUSTOM => customMessage,
                _ => customMessage is null ? "Something went wrong" : customMessage,
            };
        }
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
    public void ShowSuccess(string message = "")
    {
        message = (CultureInfo.CurrentUICulture.Name.StartsWith("fa")) ? "با موفقیت انجام شد" : "Operation Successfully Completed!";
        TempData["State"] = _success;
        TempData["Msg"] = message;
    }
    #endregion

    #region Redirection
    public IActionResult Page(object routeValue)
    {
        return RedirectToPage(routeValues: routeValue);
    }
    public IActionResult RedirectToSpecialPage(StaticPages pages, string ReturnUrl = "")
    {
        return pages switch
        {
            StaticPages.Login => Redirect($"{Url.Content("~")}{StaticValues.LoginPath}?returnUrl={ReturnUrl}"),
            StaticPages.Register => Redirect($"{Url.Content("~")}{StaticValues.RegisterPath}?returnUrl={ReturnUrl}"),
            StaticPages.Index => Redirect(Url.Content("~/")),
            _ => Redirect(Url.Content("~/")),
        };
    }
    public IActionResult RedirectToIndex()
    {
        return LocalRedirect(Url.Content("~/"));
    }
    public IActionResult RedirectToLocalIndex()
    {
        return RedirectToPage("./Index");
    }
    #endregion
    public enum StaticPages
    {
        Login,
        Register,
        Index
    }
}