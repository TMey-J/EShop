using System.Text.Json.Serialization;

namespace EShop.Application.Features.AdminPanel.Category.Requests.Commands;

public record UpdateCategoryCommandRequest:IRequest<UpdateCategoryCommandResponse>
{
    [JsonIgnore]
    [DisplayName("شناسه")]
    public long Id { get; set; }
    
    [DisplayName("عنوان جذید")]
    public string Title { get; set; } = string.Empty;
    
    [DisplayName("والد")]
    public long ParentId { get; set; }
    
    [DisplayName("والد جذیذ")]
    public string? NewPicture { get; set; }
}
public record UpdateCategoryCommandResponse;