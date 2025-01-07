using Carter;
using EShop.Application.Features.AdminPanel.Product.Requests.Commands;
using EShop.Application.Features.AdminPanel.Product.Requests.Queries;

namespace EShop.Api.Endpoints.Admin
{
    public class AdminProductEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Admin/Product").AddEndpointFilter<ApiResultEndpointFilter>();

            group.MapPost(nameof(Create), Create);
            group.MapPut(nameof(Update) + "/{id}", Update);
            group.MapGet(nameof(GetAll), GetAll);
            group.MapGet(nameof(Get) + "/{id}", Get);
        }

        #region Api Bodies

        private static async Task<IResult> Create(CreateProductCommandRequest request, IMediator mediator)
        {
            //TODO:get seller id by user claim 
            request.SellerId = 2;
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> GetAll(
            [FromBody] GetAllProductsQueryRequest request,
            IMediator mediator)
        {
            var response = await mediator.Send(request);
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> Update(long id, [FromBody] UpdateProductCommandRequest request,
            IMediator mediator)
        {
            //TODO:get seller id by user claim 
            request.SellerId = 2;
            request.Id = id;
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> Get(long id, IMediator mediator)
        {
            var response = await mediator.Send(new GetProductQueryRequest(id));
            return TypedResults.Ok(response);
        }

        #endregion
    }
}