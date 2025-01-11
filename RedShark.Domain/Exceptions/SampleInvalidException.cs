using RedShark.Shared.Abstractions.Exceptions;

namespace RedShark.Domain.Exceptions;

    public class SampleInvalidException : PublicException
    {

        public SampleInvalidException() : base("Sample ID cannot be empty.")
        {
        }
    }
