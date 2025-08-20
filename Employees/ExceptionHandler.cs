using Business.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Employees;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case NotFoundException:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return true;

            case ValidationException ve:
                var errors = ve.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = "application/problem+json";

                var pd = new HttpValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation error"
                };

                await httpContext.Response.WriteAsJsonAsync(pd, cancellationToken);
                return true;

            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return true;
        }
    }
}