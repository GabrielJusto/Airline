namespace Airline.Exceptions;

public class EntityAlreadyExistsException : AirlineException
{

    public EntityAlreadyExistsException(string entity, IReadOnlyDictionary<string, object?> logAttributes)
        : base($"{entity} already exists.", logAttributes)
    {
        
    }
}