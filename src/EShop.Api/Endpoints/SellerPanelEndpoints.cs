using EShop.Application.Features.SellerPanel.Requests.Commands;

namespace EShop.Api.Endpoints
{
    public class SellerPanelEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/sellerPanel").AddEndpointFilter<ApiResultEndpointFilter>();

            group.MapPost(nameof(ReserveProduct), ReserveProduct);
        }

        #region Api Bodies
        private static async Task<IResult> ReserveProduct(ReserveProductCommandRequest request, IMediator mediator)
        {
            //TODO: Get SellerId from claim
            request.SellerId = 2;
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        #endregion
    }
}