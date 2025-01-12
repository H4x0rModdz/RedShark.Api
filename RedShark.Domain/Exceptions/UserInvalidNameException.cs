using RedShark.Shared.Abstractions.Exceptions;

namespace RedShark.Domain.Exceptions
{
    public class UserInvalidNameException : PublicException
    {
        public UserInvalidNameException(string message) : base(message)
        {
        }

        public UserInvalidNameException() : base("Name is required.")
        {
        }

        public static UserInvalidNameException InvalidFormat() =>
            new UserInvalidNameException("Name contains invalid characters.");

        public static UserInvalidNameException NullOrWhitespace() =>
            new UserInvalidNameException("Name cannot be null or whitespace.");
    }
}
