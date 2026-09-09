using Airline.Observability;

namespace Airline.Exceptions;

public class DtoValidationException : AirlineException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public DtoValidationException(IDictionary<string, List<string>> errors)
        : base(BuildMessage(errors), BuildLogAttributes(errors))
    {
        Errors = errors.ToDictionary(entry => entry.Key, entry => entry.Value.ToArray());
    }

    private static string BuildMessage(IDictionary<string, List<string>> errors)
    {
        IEnumerable<string> described = errors.SelectMany(entry => entry.Value
            .Select(message => string.IsNullOrEmpty(entry.Key) ? message : $"{entry.Key}: {message}"));

        return "One or more validation errors occurred: " + string.Join(" | ", described);
    }
    private static Dictionary<string, object?> BuildLogAttributes(IDictionary<string, List<string>> errors)
    {
        return new Dictionary<string, object?>
        {
            [LogAttributeNames.ValidationFields] = errors.Keys.ToArray(),
        };
    }
}