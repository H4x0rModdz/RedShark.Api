using RedShark.Shared.Abstractions.Exceptions;

namespace RedShark.Domain.Exceptions
{
    public class UserInvalidEmailException : PublicException
    {
        public UserInvalidEmailException() : base("Email is required or invalid.")
        {
        }
    }
}
