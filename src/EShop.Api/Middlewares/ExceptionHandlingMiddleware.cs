using Blogger.Application.Common.Exceptions;
using EShop.Api;
using EShop.Application.Common.Exceptions;
using EShop.Application.Constants.Common;
using ValidationExp = Blog.Application.Common.Exceptions.CustomValidationException;


namespace Blogger.Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next,ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationExp exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var result = new ApiResult(false, System.Net.HttpStatusCode.BadRequest, exception.Message)
            {
                Data = exception.Errors
            };

            await context.Response.WriteAsJsonAsync(result);
        }
        catch (CustomBadRequestException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var result = new ApiResult(false, System.Net.HttpStatusCode.BadRequest,
                exception.Message)
            {
                Data = exception.Errors
            };

            await context.Response.WriteAsJsonAsync(result);
        }
        catch (NotFoundException exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            var result = new ApiResult(false, System.Net.HttpStatusCode.NotFound, exception.Message);

            await context.Response.WriteAsJsonAsync(result);
        }
        catch (DuplicateException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var result = new ApiResult(false, System.Net.HttpStatusCode.BadRequest, exception.Message);

            await context.Response.WriteAsJsonAsync(result);
        }
        catch (CustomInternalServerException exception)
        {
            _logger.LogError($"{exception.StackTrace?.Split("in")[0]}|{exception.Message}");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var result = new ApiResult(false, System.Net.HttpStatusCode.InternalServerError,
                exception.Message);

            await context.Response.WriteAsJsonAsync(result);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,$"{exception.StackTrace?.Split("in")[0]}|{exception.Message}");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var result =
                new ApiResult(false,
                    System.Net.HttpStatusCode.InternalServerError,
                    Messages.Errors.InternalServer);

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}