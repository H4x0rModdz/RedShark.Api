using RedShark.Domain.Exceptions;

namespace RedShark.Domain.ValueObjects
{
    public class UserAvatar
    {
        public string Value { get; }

        private UserAvatar(string value)
        {
            Value = value;
        }

        public static UserAvatar Create(string avatar)
        {
            if (string.IsNullOrWhiteSpace(avatar)) throw new UserInvalidAvatarException();
            
            return new UserAvatar(avatar);
        }
    }
}
