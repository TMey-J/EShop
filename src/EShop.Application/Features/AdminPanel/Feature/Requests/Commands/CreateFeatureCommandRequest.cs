using Azure;

namespace EShop.Application.Features.AdminPanel.Feature.Requests.Commands;

public record CreateFeatureCommandRequest : IRequest<CreateFeatureCommandResponse>
{
    [DisplayName("نام ویژگی")]
    public string Name { get; set; }=string.Empty;
}
public record CreateFeatureCommandResponse();