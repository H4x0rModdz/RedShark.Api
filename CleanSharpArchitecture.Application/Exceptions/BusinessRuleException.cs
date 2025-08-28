namespace CleanSharpArchitecture.Application.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException() : base("A business rule violation occurred.")
        {
        }

        public BusinessRuleException(string message) : base(message)
        {
        }

        public BusinessRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}