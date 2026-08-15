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
        bool isPersian = CultureInfo.CurrentUICulture.Name.StartsWith("fa");

        string message = errorMessage switch
        {
            ErrorMessages.NOTFOUND => isPersian ? "موردی یافت نشد!" : "No Item Found",
            ErrorMessages.ERRORHAPPEDNED => isPersian ? "خطایی رخ داد!!" : "Something went wrong",
            ErrorMessages.IDINVALID => isPersian ? "شناسه وارد شده نامعتبر است!" : "Invalid Id",
            ErrorMessages.FILLREQUESTEDDATA => isPersian ? "لطفا فیلدهای ضروری را با مقادیر صحیح پر کنید!" : "Fill requested data",
            ErrorMessages.CUSTOM => string.IsNullOrEmpty(customMessage) ? (isPersian ? "خطایی رخ داد!!" : "Something went wrong") : customMessage,
            _ => string.IsNullOrEmpty(customMessage) ? (isPersian ? "خطایی رخ داد!!" : "Something went wrong") : customMessage,
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

    public void ShowSuccess(string message = "")
    {
        if (string.IsNullOrEmpty(message))
            message = CultureInfo.CurrentUICulture.Name.StartsWith("fa") ? "با موفقیت انجام شد" : "Operation Successfully Completed!";

        TempData["State"] = _success;
        TempData["Msg"] = message;
    }
    #endregion

    #region Redirection
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
