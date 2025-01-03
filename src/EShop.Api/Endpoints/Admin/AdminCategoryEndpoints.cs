using Carter;
using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using EShop.Application.Features.AdminPanel.Category.Requests.Queries;
using EShop.Application.Features.Authorize.Requests.Commands;

namespace EShop.Api.Endpoints.Admin
{
    public class AdminCategoryEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/Admin/Category").AddEndpointFilter<ApiResultEndpointFilter>();

            group.MapPost(nameof(Create), Create);
            group.MapPost(nameof(AddFeaturesToCategory), AddFeaturesToCategory);
            group.MapPut(nameof(Update) + "/{id}", Update);
            group.MapGet(nameof(GetAll), GetAll);
            group.MapGet(nameof(GetCategoryFeatures)+ "/{categoryId}", GetCategoryFeatures);
            group.MapGet(nameof(Get) + "/{id}", Get);
        }

        #region Api Bodies
        private static async Task<IResult> Create(CreateCategoryCommandRequest request, IMediator mediator)
        {
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> Update(long id,[FromBody]UpdateCategoryCommandRequest request, IMediator mediator)
        {
            request.Id = id;
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> GetAll(
            [FromBody] GetAllCategoryQueryRequest request,
            IMediator mediator)
        {
            var response = await mediator.Send(request);
            return TypedResults.Ok(response);
        }

        private async Task<IResult> Get(long id, IMediator mediator)
        {
            var response = await mediator.Send(new GetCategoryQueryRequest() { Id = id });
            return TypedResults.Ok(response);
        }

        private static async Task<IResult> AddFeaturesToCategory(AddFeaturesToCategoryCommandRequest request,
            IMediator mediator)
        {
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        private static async Task<IResult> GetCategoryFeatures(
            long categoryId,
            IMediator mediator)
        {
            var response = await mediator.Send(new GetCategoryFeaturesQueryRequest { CategoryId = categoryId });
            return TypedResults.Ok(response);
        }

        #endregion
    }
}