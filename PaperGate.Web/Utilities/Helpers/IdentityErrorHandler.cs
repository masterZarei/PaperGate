using Microsoft.AspNetCore.Identity;

namespace PaperGate.Web.Utilities.Helpers;
public class IdentityErrorHandler : IdentityErrorDescriber
{
    public override IdentityError PasswordRequiresDigit()
    {
        return new IdentityError { Description = "رمز عبور حتما باید شامل اعداد باشد." };
    }
    public override IdentityError PasswordRequiresLower()
    {
        return new IdentityError { Description = "رمز عبور حتما باید شامل حروف کوچک باشد." };

    }
    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError { Description = "رمز عبور حتما باید شامل سمبل ها باشد." };
    }
    public override IdentityError PasswordRequiresUpper()
    {
        return new IdentityError { Description = "رمز عبور حتما باید شامل حروف بزرگ باشد." };
    }
    public override IdentityError PasswordMismatch()
    {
        return new IdentityError { Description = "رمز عبور نامعتبر است." };
    }
    public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError { Description = "رمز عبور باید حداقل 8 کاراکتر باشد." };
    }
    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
    {
        return new IdentityError { Description = "رمز عبور باید حتما شامل کاراکتر های خاص باشد." };
    }
    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError { Description = "کاربری با این نام کاربری در سیستم موجود می باشد." };
    }

}