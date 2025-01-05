namespace Point.Core.Application.Common.Exceptions
{
    public class ValidatorException : Exception
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidatorException(Dictionary<string, string[]> errors)
            : base("Invalid request values.") => Errors = errors;
    }
}
