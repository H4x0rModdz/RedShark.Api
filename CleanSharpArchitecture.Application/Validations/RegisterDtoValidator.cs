using FluentValidation;
using CleanSharpArchitecture.Application.DTOs.Auth.Request;

namespace CleanSharpArchitecture.Application.Validations
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(320).WithMessage("Email must not exceed 320 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters and spaces");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .Length(3, 30).WithMessage("Username must be between 3 and 30 characters")
                .Matches(@"^[a-zA-Z0-9_.-]+$").WithMessage("Username can only contain letters, numbers, dots, hyphens, and underscores");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .MaximumLength(128).WithMessage("Password must not exceed 128 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit")
                .Matches(@"[!@#$%^&*?._\-+=]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.Biography)
                .MaximumLength(500).WithMessage("Biography must not exceed 500 characters");

            RuleFor(x => x.ProfileImage)
                .Must(BeValidImageFile).WithMessage("Profile image must be a valid image file (jpg, jpeg, png, gif, webp)")
                .When(x => x.ProfileImage != null);

            RuleFor(x => x.State)
                .MaximumLength(100).WithMessage("State must not exceed 100 characters");

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("City must not exceed 100 characters");

            RuleFor(x => x.DateOfBirth)
                .Must(BeValidAge).WithMessage("You must be at least 13 years old")
                .When(x => x.DateOfBirth != default(DateTime));

            RuleFor(x => x.MaritalStatus)
                .MaximumLength(50).WithMessage("Marital status must not exceed 50 characters");
        }

        private bool BeValidImageFile(Microsoft.AspNetCore.Http.IFormFile? file)
        {
            if (file == null) return true; // Optional field

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            return allowedExtensions.Contains(extension) &&
                   file.ContentType.StartsWith("image/") &&
                   file.Length > 0 &&
                   file.Length <= 5 * 1024 * 1024; // 5MB max
        }

        private bool BeValidAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            
            return age >= 13;
        }
    }
}