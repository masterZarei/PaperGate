using PaperGate.Core.Libraries.StaticValues;
using System.Text.RegularExpressions;

namespace PaperGate.Core.Validations;
public static partial class Validation
{
    public static bool IsValidRole(string roleName) => roleName == Roles.AdminEndUser;
    public static bool IsDigit(string input) => input.All(char.IsDigit);
    public static bool IsValidEmail(string email) => ValidEmaillRegex().IsMatch(email);
    public static bool IsValidPostalCode(string postalCode)
    {
        if (string.IsNullOrEmpty(postalCode)) return false;
        return postalCode.Length == 10 && IsDigit(postalCode);
    }
    public static bool IsValidPersianDate(string date) => ValidPersianDate().IsMatch(date);
    public static bool IsValidExpireDate(DateTime Date) => Date >= DateTime.Now && Date < DateTime.Now.AddMonths(3);

    [GeneratedRegex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
    private static partial Regex ValidEmaillRegex();

    [GeneratedRegex("^$|^([1۱][۰-۹ 0-9]{3}[/\\/]([0 ۰][۱-۶ 1-6])[/\\/]([0 ۰][۱-۹ 1-9]|[۱۲12][۰-۹ 0-9]|[3۳][01۰۱])|[1۱][۰-۹ 0-9]{3}[/\\/]([۰0][۷-۹ 7-9]|[1۱][۰۱۲012])[/\\/]([۰0][1-9 ۱-۹]|[12۱۲][0-9 ۰-۹]|(30|۳۰)))$")]
    private static partial Regex ValidPersianDate();
}
