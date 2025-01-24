namespace EShop.Application.Features.Comment.Requests.Queries;

public record GetAllCommentsQueryRequest(long ProductId,Pagination Pagination) : IRequest<GetAllCommentsQueryResponse>;

public record GetAllCommentsQueryResponse(List<ShowCommentDto> Comments,int PageCount);