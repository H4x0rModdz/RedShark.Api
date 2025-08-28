using FluentValidation;
using CleanSharpArchitecture.Application.DTOs.Auth.Request;

namespace CleanSharpArchitecture.Application.Validations
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(320).WithMessage("Email must not exceed 320 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .MaximumLength(128).WithMessage("Password must not exceed 128 characters");
        }
    }
}