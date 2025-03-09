using FluentValidation;
using PaperGate.Core.DTOs;

namespace PaperGate.Core.Validations.UserDtosValidator;
public class LoginDtoValidation : AbstractValidator<LoginDto>
{
    public LoginDtoValidation()
    {

        RuleFor(r => r.NationalCode)
            .Matches("^[0-9]*$").WithMessage("مقدار وارد شده در فیلد کد ملی فقط باید عددی باشند")
            .NotEmpty().NotNull().WithMessage("لطفا کد ملی را وارد کنید")
            .Length(10).WithMessage("کد ملی باید 10  کاراکتر عددی باشد");

        RuleFor(r => r.Password)
            .NotEmpty().NotNull().WithMessage("لطفا رمز عبور را وارد کنید");

    }
}
