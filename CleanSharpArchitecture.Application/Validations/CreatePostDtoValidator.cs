using FluentValidation;
using CleanSharpArchitecture.Application.DTOs.Posts.Request;

namespace CleanSharpArchitecture.Application.Validations
{
    public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
    {
        private const int MaxContentLength = 2000;
        private const int MaxImagesPerPost = 10;
        private const long MaxImageSizeBytes = 10 * 1024 * 1024; // 10MB

        public CreatePostDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post content is required")
                .MaximumLength(MaxContentLength).WithMessage($"Post content must not exceed {MaxContentLength} characters");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Valid user ID is required");

            RuleFor(x => x.Images)
                .Must(BeValidImageCount).WithMessage($"Cannot upload more than {MaxImagesPerPost} images per post")
                .When(x => x.Images != null);

            RuleForEach(x => x.Images)
                .Must(BeValidImage).WithMessage("Each image must be a valid image file (jpg, jpeg, png, gif, webp) and not exceed 10MB")
                .When(x => x.Images != null);
        }

        private bool BeValidImageCount(IEnumerable<Microsoft.AspNetCore.Http.IFormFile>? images)
        {
            return images?.Count() <= MaxImagesPerPost;
        }

        private bool BeValidImage(Microsoft.AspNetCore.Http.IFormFile? image)
        {
            if (image == null) return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(extension) &&
                   image.ContentType.StartsWith("image/") &&
                   image.Length > 0 &&
                   image.Length <= MaxImageSizeBytes;
        }
    }
}