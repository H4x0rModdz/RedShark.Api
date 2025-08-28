using System.Text.RegularExpressions;

namespace CleanSharpArchitecture.Domain.ValueObjects
{
    public sealed record Email
    {
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            var normalizedEmail = email.Trim().ToLowerInvariant();

            if (normalizedEmail.Length > 320) // RFC 5321 limit
                throw new ArgumentException("Email address is too long", nameof(email));

            if (!EmailRegex.IsMatch(normalizedEmail))
                throw new ArgumentException("Invalid email format", nameof(email));

            return new Email(normalizedEmail);
        }

        public static implicit operator string(Email email) => email.Value;

        public override string ToString() => Value;
    }
}