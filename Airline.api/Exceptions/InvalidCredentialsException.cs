using Airline.Observability;

namespace Airline.Exceptions;

public class InvalidCredentialsException : AirlineException
{
    private const string DefaultMessage = "Invalid email or password.";

    public InvalidCredentialsException(string email)
        : base(DefaultMessage, new Dictionary<string, object?>
        {
            [LogAttributeNames.UserEmail] = email,
        })
    {
    }
}
