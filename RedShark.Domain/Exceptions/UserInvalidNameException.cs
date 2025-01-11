using RedShark.Shared.Abstractions.Exceptions;

namespace RedShark.Domain.Exceptions
{
    public class UserInvalidNameException : PublicException
    {
        public UserInvalidNameException() : base("Name is required.")
        {
        }
    }
}
