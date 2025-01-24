using EShop.Application.Features.Product.Requests.Queries;

namespace EShop.Api.Endpoints
{
    public class ProductEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/product").AddEndpointFilter<ApiResultEndpointFilter>();
            
            group.MapGet(nameof(ShowProduct)+"/{id}", ShowProduct);
            group.MapGet(nameof(GetSellers)+"/{productId}/{colorCode}", GetSellers);
        }

        #region Api Bodies
        private static async Task<IResult> ShowProduct(long id, IMediator mediator)
        {
            var response= await mediator.Send(new ShowProductQueryRequest(id));
            return TypedResults.Ok(response);
        }
        private static async Task<IResult> GetSellers(long productId,string colorCode, IMediator mediator)
        {
            var response= await mediator.Send(new GetSellersProductQueryRequest(productId, colorCode));
            return TypedResults.Ok(response);
        }
        #endregion
    }
}