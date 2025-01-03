using Carter;
using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;
using EShop.Application.Features.AdminPanel.Tag.Requests.Queries;
using EShop.Application.Features.AdminPanel.User.Requests.Commands;
using EShop.Application.Features.AdminPanel.User.Requests.Queries;
using EShop.Application.Features.Authorize.Requests.Commands;

namespace EShop.Api.Endpoints.Admin
{
    public class AdminTagEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Admin/Tag").AddEndpointFilter<ApiResultEndpointFilter>();

            group.MapPost(nameof(Create), Create);
            group.MapPut(nameof(Update)+ "/{id}", Update);
            group.MapGet(nameof(GetAll), GetAll);
            group.MapGet(nameof(Get) + "/{id}", Get);
        }

        #region Api Bodies
        private static async Task<IResult> Create(CreateTagCommandRequest request, IMediator mediator)
        {
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> Update(long id,[FromBody]UpdateTagCommandRequest request, IMediator mediator)
        {
            request.Id = id;
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> GetAll(
            [FromBody] GetAllTagsQueryRequest request,
            IMediator mediator)
        {
            var response = await mediator.Send(request);
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> Get(long id, IMediator mediator)
        {
            var response = await mediator.Send(new GetTagQueryRequest { Id = id });
            return TypedResults.Ok(response);
        }


        #endregion
    }
}