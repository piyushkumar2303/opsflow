using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpsFlow.Application.Common.Exceptions;

namespace OpsFlow.Api.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;

        switch (exception)
        {
            case ValidationException validationException:
                {
                    _logger.LogWarning(
                        "Validation failed. TraceId: {TraceId}",
                        httpContext.TraceIdentifier);

                    var errors = validationException.Errors
                        .GroupBy(error => error.PropertyName)
                        .ToDictionary(
                            group => group.Key,
                            group => group
                                .Select(error => error.ErrorMessage)
                                .Distinct()
                                .ToArray());

                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation Error",
                        Detail = "One or more validation errors occurred.",
                        Instance = httpContext.Request.Path
                    };

                    problemDetails.Extensions["errors"] = errors;

                    break;
                }

            case ConflictException conflictException:
                {
                    _logger.LogWarning(
                        exception,
                        "A conflict occurred. TraceId: {TraceId}",
                        httpContext.TraceIdentifier);

                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Conflict",
                        Detail = conflictException.Message,
                        Instance = httpContext.Request.Path
                    };

                    break;
                }

            case UnauthorizedException unauthorizedException:
                {
                    _logger.LogWarning(
                        "Unauthorized request. TraceId: {TraceId}",
                        httpContext.TraceIdentifier);

                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Title = "Unauthorized",
                        Detail = unauthorizedException.Message,
                        Instance = httpContext.Request.Path
                    };

                    break;
                }

            default:
                {
                    _logger.LogError(
                        exception,
                        "Unhandled exception occurred. TraceId: {TraceId}",
                        httpContext.TraceIdentifier);

                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Internal Server Error",
                        Detail = "An unexpected error occurred while processing your request.",
                        Instance = httpContext.Request.Path
                    };

                    break;
                }
        }

        problemDetails.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        httpContext.Response.StatusCode =
            problemDetails.Status
            ?? StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}