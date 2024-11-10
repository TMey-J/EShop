namespace EShop.Application.Features.AdminPanel.Requests.Commands.Tag;

public record UpdateTagCommandRequest:IRequest<UpdateTagCommandResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; set; }
    
    [DisplayName("عنوان")]
    public string Title { get; set; } = string.Empty;
}
public record UpdateTagCommandResponse;
