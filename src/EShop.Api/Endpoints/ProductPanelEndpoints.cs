using EShop.Application.Features.Product.Requests.Queries;

namespace EShop.Api.Endpoints
{
    public class ProductEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/product").AddEndpointFilter<ApiResultEndpointFilter>();
            
            group.MapGet(nameof(ShowProduct)+"/{id}", ShowProduct);
        }

        #region Api Bodies
        private static async Task<IResult> ShowProduct(long id, IMediator mediator)
        {
            var response= await mediator.Send(new ShowProductQueryRequest(id));
            return TypedResults.Ok(response);
        }
        #endregion
    }
}