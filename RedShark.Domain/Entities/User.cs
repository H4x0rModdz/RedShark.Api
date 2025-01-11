using RedShark.Domain.Exceptions;
using RedShark.Domain.ValueObjects;

namespace RedShark.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public UserEmail Email { get; private set; }
        public string Password { get; private set; }
        public string? Biography { get; private set; }
        public UserAvatar Avatar { get; private set; }

        private User(string name, UserEmail email, string password, UserAvatar avatar, string? biography = null)
        {
            ValidateName(name);
            Name = name;
            Email = email;
            Password = password;
            Avatar = avatar;
            Biography = biography;
        }

        public static User Create(string name, string email, string password, string avatar, string? biography = null)
        {
            var emailValue = UserEmail.Create(email);
            var avatarValue = UserAvatar.Create(avatar);

            ValidateName(name);
            ValidatePassword(password);

            return new User(name, emailValue, password, avatarValue, biography);
        }

        public void UpdateProfile(string name, string? biography, string avatar)
        {
            ValidateName(name);
            Avatar = UserAvatar.Create(avatar);

            Name = name;
            Biography = biography;

            UpdateTimestamp();
        }

        public void UpdatePassword(string newPassword)
        {
            ValidatePassword(newPassword);

            Password = newPassword;
            UpdateTimestamp();
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new UserInvalidNameException();
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new UserInvalidPasswordException();
        }

        public void DeleteAccount()
        {
            MarkAsDeleted();
        }
    }
}
