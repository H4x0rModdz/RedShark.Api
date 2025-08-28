using System.Text.RegularExpressions;

namespace CleanSharpArchitecture.Domain.ValueObjects
{
    public sealed record Username
    {
        private static readonly Regex UsernameRegex = new(
            @"^[a-zA-Z0-9_.-]+$",
            RegexOptions.Compiled
        );

        private static readonly string[] ReservedUsernames = 
        {
            "admin", "administrator", "root", "system", "support", "help",
            "api", "www", "mail", "email", "test", "demo", "null", "undefined"
        };

        public string Value { get; }

        private Username(string value)
        {
            Value = value;
        }

        public static Username Create(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            var trimmedUsername = username.Trim();

            if (trimmedUsername.Length < 3)
                throw new ArgumentException("Username must be at least 3 characters long", nameof(username));

            if (trimmedUsername.Length > 30)
                throw new ArgumentException("Username must not exceed 30 characters", nameof(username));

            if (!UsernameRegex.IsMatch(trimmedUsername))
                throw new ArgumentException("Username can only contain letters, numbers, dots, hyphens, and underscores", nameof(username));

            if (ReservedUsernames.Contains(trimmedUsername.ToLowerInvariant()))
                throw new ArgumentException("This username is reserved and cannot be used", nameof(username));

            return new Username(trimmedUsername);
        }

        public static implicit operator string(Username username) => username.Value;

        public override string ToString() => Value;
    }
}