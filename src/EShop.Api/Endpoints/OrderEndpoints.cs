using EShop.Application.Features.Order.Requests.Command;

namespace EShop.Api.Endpoints
{
    public class OrderEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/order").AddEndpointFilter<ApiResultEndpointFilter>();
            
            group.MapPost(nameof(Add), Add);
        }

        #region Api Bodies
        private static async Task<IResult> Add(AddToOrderCommandRequest request,IMediator mediator)
        {
            request.UserId = 3;
            await mediator.Send(request);
            return TypedResults.Ok();
        }
        #endregion
    }
}