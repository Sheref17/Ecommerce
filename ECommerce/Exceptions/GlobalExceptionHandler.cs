using ECommerce.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Exceptions
{

    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception,"An unhandled exception occurred.");

            if (exception is FluentValidation.ValidationException validationException)
            {
                var errors = validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(x => x.ErrorMessage)
                            .ToArray());

                var validationProblem = new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "One or more validation errors occurred."
                };

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                await httpContext.Response.WriteAsJsonAsync(validationProblem,cancellationToken);

                return true;
            }

            if (exception is DomainException domainException)
            {
                var domainProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "A domain error occurred.",
                    Detail = domainException.Message
                };

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                await httpContext.Response.WriteAsJsonAsync(domainProblemDetails,
                    cancellationToken);

                return true;
            }

            if (exception is KeyNotFoundException keyNotFoundException)
            {
                var notFoundProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found.",
                    Detail = keyNotFoundException.Message
                };

                httpContext.Response.StatusCode =StatusCodes.Status404NotFound;

                await httpContext.Response.WriteAsJsonAsync(notFoundProblemDetails,
                    cancellationToken);

                return true;
            }

       
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = "An error occurred while processing your request."
            };

            httpContext.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(problemDetails,cancellationToken);

            return true;
        }
    }
}
