using FluentValidation;
using InnoShop.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics; 
using Microsoft.AspNetCore.Http;       
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding; 
namespace InnoShop.Api.Infrastructure.ExceptionHandling
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

                case NotFoundException notFoundEx:
                    var notFoundProblem = new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Not Found",
                        Detail = notFoundEx.Message,
                        Instance = context.Request.Path
                    };
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsJsonAsync(notFoundProblem, token);
                    return true;

                case ForbiddenException forbiddenEx:
                    var forbiddenProblem = new ProblemDetails
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Title = "Forbidden",
                        Detail = forbiddenEx.Message,
                        Instance = context.Request.Path
                    };
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(forbiddenProblem, token);
                    return true;

                default:
                    return false;
            }
        }
    }
}