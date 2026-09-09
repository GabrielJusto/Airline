using System.ComponentModel.DataAnnotations;

namespace Horta.Application.Validation;

public static class ValidationErrors
{
    public const string ModelLevel = "";

    public static Dictionary<string, List<string>> FromResults(IEnumerable<ValidationResult> validationResults)
    {
        Dictionary<string, List<string>> errors = new();

        foreach(ValidationResult result in validationResults)
        {
            string message = result.ErrorMessage ?? "Invalid value.";
            string[] fields = (result.MemberNames ?? [])
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToArray();

            if(fields.Length == 0)
            {
                fields = [ModelLevel];
            }

            foreach(string field in fields)
            {
                errors.Add(field, message);
            }
        }

        return errors;
    }

    public static void Add(this Dictionary<string, List<string>> errors, string field, string message)
    {
        if(!errors.TryGetValue(field, out List<string>? messages))
        {
            messages = new List<string>();
            errors[field] = messages;
        }

        messages.Add(message);
    }

    public static void AddRange(this Dictionary<string, List<string>> errors, string field, IEnumerable<string> messages)
    {
        foreach(string message in messages)
        {
            errors.Add(field, message);
        }
    }
}
