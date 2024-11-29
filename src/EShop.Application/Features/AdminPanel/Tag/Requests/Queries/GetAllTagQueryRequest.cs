namespace EShop.Application.Features.AdminPanel.Tag.Requests.Queries;

public record GetAllTagsQueryRequest(SearchTagDto Search):IRequest<GetAllTagsQueryResponse>;
public record GetAllTagsQueryResponse(List<ShowTagDto> Tags,SearchTagDto Search,int PageCount);