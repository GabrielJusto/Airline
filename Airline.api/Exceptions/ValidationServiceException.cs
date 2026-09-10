namespace Airline.Exceptions;

public class ValidationServiceException : Exception
{
    public IList<AirlineException> Exceptions { get; }
    public string[] Errors { get; }

    public ValidationServiceException(
        IList<AirlineException> exceptions,
        string[] errors
    ) : base("Validation failed.")
    {
        Exceptions = exceptions;
        Errors = errors;
    }
}