using System.Net;

namespace EShop.Api;

public class ApiResultEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);
        object? data = null;
        var value = result?.GetType().GetProperty("Value")?.GetValue(result);
        if (value is not null)
        {
            data = value;
        }
        return new ApiResult(true, HttpStatusCode.OK, data: data );
    }
}