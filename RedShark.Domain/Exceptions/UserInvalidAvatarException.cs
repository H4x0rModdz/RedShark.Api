using RedShark.Shared.Abstractions.Exceptions;

namespace RedShark.Domain.Exceptions
{
    public class UserInvalidAvatarException : PublicException
    {
        public UserInvalidAvatarException() : base("Avatar is required.")
        {
        }
    }
}
