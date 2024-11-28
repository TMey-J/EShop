using System.Net;

namespace EShop.Api;

public class ApiResultEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);
        object? data = null;
        if (result is Ok<object> obj)
        {
            data= obj.Value;
        }
        return new ApiResult(true, HttpStatusCode.OK, data: data );
    }
}