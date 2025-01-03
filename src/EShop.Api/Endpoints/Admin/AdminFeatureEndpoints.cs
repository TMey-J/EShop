using Carter;
using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Application.Features.AdminPanel.Feature.Requests.Commands;
using EShop.Application.Features.AdminPanel.Feature.Requests.Queries;
using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Application.Features.AdminPanel.User.Requests.Commands;
using EShop.Application.Features.AdminPanel.User.Requests.Queries;
using EShop.Application.Features.Authorize.Requests.Commands;

namespace EShop.Api.Endpoints.Admin
{
    public class AdminFeatureEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Admin/Feature").AddEndpointFilter<ApiResultEndpointFilter>();

            group.MapPost(nameof(Create), Create);
            group.MapPut(nameof(Update)+ "/{id}", Update);
            group.MapGet(nameof(GetAll), GetAll);
            group.MapGet(nameof(Get) + "/{id}", Get);
        }

        #region Api Bodies
        private static async Task<IResult> Create(CreateFeatureCommandRequest request, IMediator mediator)
        {
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> Get(long id, IMediator mediator)
        {
            var response = await mediator.Send(new GetFeatureQueryRequest { Id = id });
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> Update(long id,[FromBody]UpdateFeatureCommandRequest request, IMediator mediator)
        {
            request.Id = id;
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> GetAll(
            [FromBody] GetAllFeaturesQueryRequest request,
            IMediator mediator)
        {
            var response = await mediator.Send(request);
            return TypedResults.Ok(response);
        }

        #endregion
    }
}