using System.ComponentModel.DataAnnotations;

using Airline.Exceptions;

namespace Airline.Validators;

public class ValidatorService(IValidation[] validations)
{
    private readonly IValidation[] _validations = validations;
    private readonly IList<AirlineException> _exceptions = [];
    private string[] errors = [];
    public void Validate()
    {
        foreach(IValidation validation in _validations)
        {
            try
            {
                validation.Validate();
            }
            catch(AirlineException ex)
            {
                _exceptions.Add(ex);
                this.errors = this.errors.Append(ex.Message).ToArray();
            }

        }
    }

    public void ValidateAndThrow()
    {
        Validate();
        if(HasErrors())
        {
            throw new ValidationServiceException(_exceptions, GetErrors());
        }
    }

    public bool HasErrors()
    {
        return this.errors.Length > 0;
    }

    public string[] GetErrors()
    {
        return this.errors;
    }
}