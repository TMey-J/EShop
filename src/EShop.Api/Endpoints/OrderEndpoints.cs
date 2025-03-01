using EShop.Application.Features.Order.Requests.Command;
using EShop.Application.Features.Order.Requests.Queries;

namespace EShop.Api.Endpoints
{
    public class OrderEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/order").AddEndpointFilter<ApiResultEndpointFilter>();
            
            group.MapPost(nameof(Add), Add);
            group.MapPost(nameof(ChangeCount), ChangeCount);
            group.MapGet(nameof(ShowOrders), ShowOrders);
            group.MapPost(nameof(Pay), Pay);
            group.MapPost(nameof(Verify), Verify);
        }

        #region Api Bodies
        private static async Task<IResult> Add(AddToOrderCommandRequest request,IMediator mediator)
        {
            //TODO:get userId from claim
            request.UserId = 1;
            await mediator.Send(request);
            return TypedResults.Ok();
        }
        
        private static async Task<IResult> ChangeCount(ChangeOrderCountCommandRequest request,IMediator mediator)
        {
            //TODO:get userId from claim
            request.UserId = 1;
            await mediator.Send(request);
            return TypedResults.Ok();
        }
        
        private static async Task<IResult> ShowOrders(IMediator mediator)
        {
            //TODO:get userId from claim
            var userId = 1;
            var response= await mediator.Send(new GetAllOrdersQueryRequest{UserId = userId});
            return TypedResults.Ok(response);
        }
        private static async Task<IResult> Pay(PayOrderCommandRequest request,IMediator mediator)
        {
            //TODO:get userId from claim
            request.UserId= 1;
            var response= await mediator.Send(request);
            return TypedResults.Ok(response);
        }
        private static async Task<IResult> Verify(VerifyOrderPaymentCommandRequest request,IMediator mediator)
        {
            var response= await mediator.Send(request);
            return TypedResults.Ok(response);
        }
        #endregion
    }
}