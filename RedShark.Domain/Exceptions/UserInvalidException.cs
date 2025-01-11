using RedShark.Shared.Abstractions.Exceptions;

namespace RedShark.Domain.Exceptions
{
    public class UserInvalidException : PublicException
    {
        public UserInvalidException(string message)
            : base(message)
        {
        }
    }
}
