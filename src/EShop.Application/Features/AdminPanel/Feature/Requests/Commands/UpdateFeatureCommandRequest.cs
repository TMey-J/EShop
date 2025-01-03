using System.Text.Json.Serialization;

namespace EShop.Application.Features.AdminPanel.Feature.Requests.Commands;

public record UpdateFeatureCommandRequest:IRequest<UpdateFeatureCommandResponse>
{
    [JsonIgnore]
    [DisplayName("شناسه")]
    public long Id { get; set; }
    
    [DisplayName("عنوان")]
    public string Name { get; set; } = string.Empty;
}

public record UpdateFeatureCommandResponse;
