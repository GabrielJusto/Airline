using Airline.Observability;

namespace Airline.Exceptions;

public class EntityNotFoundException : AirlineException
{
    public string EntityName { get; }
    public object Key { get; }

    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.", new Dictionary<string, object?>
        {
            [LogAttributeNames.EntityName] = entityName,
            [LogAttributeNames.EntityKey] = key,
        })
    {
        EntityName = entityName;
        Key = key;
    }

    public EntityNotFoundException(Type entityType, object key)
        : this(entityType.Name, key)
    {
    }
}