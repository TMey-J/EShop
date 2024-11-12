namespace EShop.Application.Features.AdminPanel.Tag.Requests.Queries;

public record GetAllTagQueryRequest(SearchTagDto Search):IRequest<GetAllTagQueryResponse>;
public record GetAllTagQueryResponse(List<ShowTagDto> tags,SearchTagDto Search,int pageCount);