using FluentValidation;
using InnoShop.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics; 
using Microsoft.AspNetCore.Http;      
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding; 

namespace InnoShop.UserApi.Infrastructure.ExceptionHandling
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            return await HandleExceptionAsync(httpContext, exception, cancellationToken);
        }

        private async Task<bool> HandleExceptionAsync(HttpContext context, Exception exception, CancellationToken token)
        {
            switch (exception)
            {
                case ValidationException validationEx:
                    var modelState = new ModelStateDictionary();
                    foreach (var error in validationEx.Errors)
                    {
                        modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    var validationProblem = new ValidationProblemDetails(modelState)
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        Title = "One or more validation errors occurred.",
                        Status = StatusCodes.Status400BadRequest,
                    };

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(validationProblem, token);
                    return true;

                case AuthException authEx:
                    var authProblem = new ProblemDetails
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                        Title = "Unauthorized",
                        Detail = authEx.Message,
                        Instance = context.Request.Path
                    };

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(authProblem, token);
                    return true;

                case ConflictException conflictEx:
                    var conflictProblem = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Conflict",
                        Detail = conflictEx.Message,
                        Instance = context.Request.Path
                    };
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    await context.Response.WriteAsJsonAsync(conflictProblem, token);
                    return true;

                case InvalidLinkException invalidLinkEx:
                    var linkProblem = new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Bad Request",
                        Detail = invalidLinkEx.Message,
                        Instance = context.Request.Path
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(linkProblem, token);
                    return true;

                default:
                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "An error occurred while processing your request.",
                        Detail = exception.Message, 
                        Instance = context.Request.Path
                    };

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(problemDetails, token);

                    return true;
            }
        }
    }
}