using EShop.Application.Features.Comment.Requests.Command;

namespace EShop.Api.Endpoints
{
    public class CommentEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/comment").AddEndpointFilter<ApiResultEndpointFilter>();

            group.MapPost(nameof(Create), Create);
        }

        #region Api Bodies

        private static async Task<IResult> Create(CreateCommentCommandRequest request, IMediator mediator)
        {
            await mediator.Send(request);
            return TypedResults.Ok();
        }

        #endregion
    }
}