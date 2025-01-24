using EShop.Application.Features.Comment.Requests.Command;
using EShop.Application.Features.Comment.Requests.Queries;

namespace EShop.Api.Endpoints
{
    public class CommentEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/comment").AddEndpointFilter<ApiResultEndpointFilter>();

            group.MapPost(nameof(Create), Create);
            group.MapGet(nameof(GetAll), GetAll);
        }

        #region Api Bodies

        private static async Task<IResult> Create(CreateCommentCommandRequest request, IMediator mediator)
        {
            await mediator.Send(request);
            return TypedResults.Ok();
        }
        private static async Task<IResult> GetAll([FromBody]GetAllCommentsQueryRequest request, IMediator mediator)
        {
            var response= await mediator.Send(request);
            return TypedResults.Ok(response);
        }
        #endregion
    }
}