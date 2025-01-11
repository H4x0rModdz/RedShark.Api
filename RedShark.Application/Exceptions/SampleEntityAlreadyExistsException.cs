using RedShark.Shared.Abstractions.Exceptions;

namespace RedShark.Application.Exceptions;

    public class SampleEntityAlreadyExistsException : PublicException
    {
        public string Name { get; }

        public SampleEntityAlreadyExistsException(string name)
            : base($"sampleEntity with name '{name}' already exists.")
        {
            Name = name;
        }
    }
