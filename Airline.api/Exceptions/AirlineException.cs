namespace Airline.Exceptions;

public class AirlineException : Exception
{
    private static readonly IReadOnlyDictionary<string, object?> NoAttributes = new Dictionary<string, object?>();
    public IReadOnlyDictionary<string, object?> LogAttributes { get; }

    protected AirlineException(string message, IReadOnlyDictionary<string, object?>? logAttributes = null)
    : base(message)
    {
        LogAttributes = logAttributes ?? NoAttributes;
    }
}