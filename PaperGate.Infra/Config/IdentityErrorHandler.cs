using Microsoft.AspNetCore.Identity;

namespace PaperGate.Infra.Config;
public class IdentityErrorHandler : IdentityErrorDescriber
{
    public override IdentityError PasswordRequiresDigit() => new() { Description = "رمز عبور حتما باید شامل اعداد باشد." };

    public override IdentityError PasswordRequiresLower() => new() { Description = "رمز عبور حتما باید شامل حروف کوچک باشد." };

    public override IdentityError PasswordRequiresNonAlphanumeric() => new() { Description = "رمز عبور حتما باید شامل سمبل ها باشد." };

    public override IdentityError PasswordRequiresUpper() => new() { Description = "رمز عبور حتما باید شامل حروف بزرگ باشد." };

    public override IdentityError PasswordMismatch() => new() { Description = "رمز عبور نامعتبر است." };

    public override IdentityError PasswordTooShort(int length) => new() { Description = "رمز عبور باید حداقل 8 کاراکتر باشد." };

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => new() { Description = "رمز عبور باید حتما شامل کاراکتر های خاص باشد." };

    public override IdentityError DuplicateUserName(string userName) => new() { Description = "کاربری با این نام کاربری در سیستم موجود می باشد." };


}