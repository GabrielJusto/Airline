namespace Airline.Exceptions;

public class ValidationServiceException(string[] errors) : Exception
{
    public string[] Errors = errors;
}