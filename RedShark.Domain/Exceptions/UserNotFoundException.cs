using RedShark.Shared.Abstractions.Exceptions;

namespace RedShark.Domain.Exceptions
{
    public class UserNotFoundException : PublicException
    {
        public string Email { get; }

        public UserNotFoundException(string email)
            : base($"User with email '{email}' not found.")
        {
            Email = email;
        }
    }
}
