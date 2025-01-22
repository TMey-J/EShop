using EShop.Application.Features.SellerPanel.Requests.Commands;
using EShop.Application.Features.SellerPanel.Requests.Queries;

namespace EShop.Api.Endpoints
{
    public class SellerPanelEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/sellerPanel").AddEndpointFilter<ApiResultEndpointFilter>();

            group.MapPost(nameof(ReserveProduct), ReserveProduct);
            group.MapPut(nameof(UpdateReservedProduct), UpdateReservedProduct);
            group.MapGet(nameof(SearchProduct)+"/{title}", SearchProduct);
            group.MapGet(nameof(ShowProduct)+"/{id}", ShowProduct);
            group.MapGet(nameof(GetAllReserves), GetAllReserves);
        }

        #region Api Bodies
        private static async Task<IResult> ReserveProduct(ReserveProductCommandRequest request, IMediator mediator)
        {
            //TODO: Get SellerId from claim
            request.SellerId = 3;
            await mediator.Send(request);
            return TypedResults.Ok();
        }
        private static async Task<IResult> UpdateReservedProduct(UpdateReservedProductCommandRequest request, IMediator mediator)
        {
            //TODO: Get SellerId from claim
            request.SellerId = 3;
            await mediator.Send(request);
            return TypedResults.Ok();
        }
        private static async Task<IResult> GetAllReserves([FromBody]GetAllReservedProductsQueryRequest request, IMediator mediator)
        {
            //TODO: Get SellerId from claim
            request.SellerId = 3;
            var response= await mediator.Send(request);
            return TypedResults.Ok(response);
        }
        private static async Task<IResult> SearchProduct(string title, IMediator mediator)
        {
           var response= await mediator.Send(new SearchProductQueryRequest{Title = title});
            return TypedResults.Ok(response);
        }
        private static async Task<IResult> ShowProduct(long id, IMediator mediator)
        {
            var response= await mediator.Send(new ShowProductQueryRequest{Id = id});
            return TypedResults.Ok(response);
        }
        #endregion
    }
}