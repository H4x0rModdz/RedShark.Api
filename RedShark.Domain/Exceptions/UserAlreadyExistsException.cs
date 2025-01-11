using RedShark.Shared.Abstractions.Exceptions;

namespace RedShark.Domain.Exceptions
{
    public class UserAlreadyExistsException : PublicException
    {
        public string Email { get; }

        public UserAlreadyExistsException(string email)
            : base($"User with email '{email}' already exists.")
        {
            Email = email;
        }
    }
}
