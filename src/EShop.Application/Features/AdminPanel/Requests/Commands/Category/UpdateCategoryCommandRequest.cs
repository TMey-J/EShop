namespace EShop.Application.Features.AdminPanel.Requests.Commands.Category;

public record UpdateCategoryCommandRequest:IRequest<UpdateCategoryCommandResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; set; }
    
    [DisplayName("عنوان جذید")]
    public string NewTitle { get; set; } = string.Empty;
    
    [DisplayName("والد جذیذ")]
    public long? NewParentId { get; set; }
    
    [DisplayName("والد جذیذ")]
    public string? NewPicture { get; set; }
}
public record UpdateCategoryCommandResponse;