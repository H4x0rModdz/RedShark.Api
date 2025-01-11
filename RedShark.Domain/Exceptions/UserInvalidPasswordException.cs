using RedShark.Shared.Abstractions.Exceptions;

namespace RedShark.Domain.Exceptions
{
    public class UserInvalidPasswordException : PublicException
    {
        public UserInvalidPasswordException() : base("Password is required.")
        {
        }
    }
}
