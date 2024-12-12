namespace EShop.Application.Features.AdminPanel.Feature.Requests.Queries;

public record GetAllFeaturesQueryRequest(SearchFeatureDto? Search):IRequest<GetAllFeaturesQueryResponse>;
public record GetAllFeaturesQueryResponse(List<ShowFeatureDto> Features,SearchFeatureDto? Search,int? PageCount);