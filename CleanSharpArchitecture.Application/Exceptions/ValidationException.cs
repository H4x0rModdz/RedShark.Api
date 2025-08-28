using System.ComponentModel.DataAnnotations;

namespace CleanSharpArchitecture.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<ValidationResult> ValidationResults { get; }

        public ValidationException() : base("One or more validation errors occurred.")
        {
            ValidationResults = Enumerable.Empty<ValidationResult>();
        }

        public ValidationException(string message) : base(message)
        {
            ValidationResults = Enumerable.Empty<ValidationResult>();
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
            ValidationResults = Enumerable.Empty<ValidationResult>();
        }

        public ValidationException(IEnumerable<ValidationResult> validationResults) 
            : base($"Validation failed: {string.Join(", ", validationResults.Select(x => x.ErrorMessage))}")
        {
            ValidationResults = validationResults;
        }

        public ValidationException(string message, IEnumerable<ValidationResult> validationResults) : base(message)
        {
            ValidationResults = validationResults;
        }
    }
}