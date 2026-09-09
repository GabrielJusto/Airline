using System.Diagnostics;
using System.Security.Claims;

using Airline.Observability;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Airline.Exceptions.Handler;

public class ExceptionHandler : IExceptionHandler
{
    private const string ServerErrorDetail = "An unexpected error occurred while processing the request.";

    private readonly ILogger<ExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public ExceptionHandler(ILogger<ExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        (int status, LogLevel level) = Classify(exception);

        LogFailure(exception, status, level, httpContext);

        Activity.Current?.SetStatus(ActivityStatusCode.Error, exception.GetType().Name);
        Activity.Current?.AddException(exception);

        httpContext.Response.StatusCode = status;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = BuildProblemDetails(exception, status),
        });

    }

    private static (int Status, LogLevel Level) Classify(Exception exception)
    {
        return exception switch
        {
            TicketPurchaseException => (StatusCodes.Status400BadRequest, LogLevel.Warning),
            ValidationServiceException => (StatusCodes.Status400BadRequest, LogLevel.Warning),
            EntityNotFoundException => (StatusCodes.Status404NotFound, LogLevel.Warning),
            _ => (StatusCodes.Status500InternalServerError, LogLevel.Error),
        };
    }


    private void LogFailure(Exception exception, int status, LogLevel level, HttpContext httpContext)
    {
        Dictionary<string, object?> attributes = exception is AirlineException horta
            ? new Dictionary<string, object?>(horta.LogAttributes)
            : [];

        string? userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(!string.IsNullOrWhiteSpace(userId))
        {
            attributes[LogAttributeNames.UserId] = userId;
        }

        using(_logger.BeginScope(attributes))
        {
            _logger.Log(level, exception,
                "Request failed. {error.type} {http.response.status_code}",
                exception.GetType().Name, status);
        }
    }

    private static ProblemDetails BuildProblemDetails(Exception exception, int status)
    {
        ProblemDetails problem = new()
        {
            Status = status,
            Detail = status >= StatusCodes.Status500InternalServerError ? ServerErrorDetail : exception.Message,
        };

        if(exception is DtoValidationException validation)
        {
            problem.Extensions["errors"] = validation.Errors;
        }

        return problem;
    }
}