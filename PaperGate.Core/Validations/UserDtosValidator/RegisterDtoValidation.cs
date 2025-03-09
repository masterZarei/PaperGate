using FluentValidation;
using PaperGate.Core.DTOs;

namespace PaperGate.Core.Validations.UserDtosValidator;
public class RegisterDtoValidation : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidation()
    {
        RuleFor(r => r.Name)
            .NotEmpty().NotNull().WithMessage("لطفا نام را وارد کنید")
            .MaximumLength(100).WithMessage("نام نمی تواند بیشتر از 100 کاراکتر باشد");

        RuleFor(r => r.LastName)
            .NotEmpty().NotNull().WithMessage("لطفا نام خانوادگی را وارد کنید")
            .MaximumLength(100).WithMessage("نام خانوادگی نمی تواند بیشتر از 100 کاراکتر باشد");

        RuleFor(r => r.NationalCode)
            .Matches("^[0-9]*$").WithMessage("مقدار وارد شده در فیلد کد ملی فقط باید عددی باشند")
            .NotEmpty().NotNull().WithMessage("لطفا کد ملی را وارد کنید")
            .Length(10).WithMessage("کد ملی باید 10  کاراکتر عددی باشد");

        RuleFor(r => r.Password)
            .NotEmpty().NotNull().WithMessage("لطفا رمز عبور را وارد کنید");

        RuleFor(r => r.ConfirmPassword)
            .NotEmpty().NotNull().WithMessage("لطفا تکرار رمز عبور را وارد کنید")
            .Equal(a => a.Password)
            .When(a => !string.IsNullOrWhiteSpace(a.Password))
            .WithMessage("رمز عبور و تکرار رمز عبور باید با هم همخوانی داشته باشند");

    }
}
