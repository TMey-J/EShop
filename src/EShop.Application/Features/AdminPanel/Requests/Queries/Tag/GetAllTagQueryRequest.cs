using EShop.Application.DTOs;

namespace EShop.Application.Features.AdminPanel.Requests.Queries.Tag;

public record GetAllTagQueryRequest(SearchTagDto Search):IRequest<GetAllTagQueryResponse>;
public record GetAllTagQueryResponse(List<ShowTagDto> tags,SearchTagDto Search,int pageCount);