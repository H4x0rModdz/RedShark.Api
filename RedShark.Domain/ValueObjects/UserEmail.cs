using RedShark.Domain.Exceptions;

namespace RedShark.Domain.ValueObjects
{
    public class UserEmail
    {
        public string Value { get; }

        private UserEmail(string value)
        {
            Value = value;
        }

        public static UserEmail Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) throw new UserInvalidEmailException();
            
            return new UserEmail(email);
        }
    }
}