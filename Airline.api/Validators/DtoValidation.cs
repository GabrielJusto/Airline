using System.ComponentModel.DataAnnotations;

using Airline.Exceptions;

using Horta.Application.Validation;

namespace Airline.Validators;

public class DtoValidation : IValidation
{
    private readonly object Data;

    public DtoValidation(object data)
    {
        Data = data;
    }
    public void Validate()
    {
        ValidationContext validationContext = new(Data);
        List<ValidationResult> validationResults = new();

        bool isValid = Validator.TryValidateObject(Data, validationContext, validationResults, validateAllProperties: true);

        if(!isValid)
        {
            throw new DtoValidationException(ValidationErrors.FromResults(validationResults));
        }
    }
}