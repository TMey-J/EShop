namespace EShop.Application.Features.AdminPanel.Tag.Requests.Commands;

public record UpdateTagCommandRequest:IRequest<UpdateTagCommandResponse>
{
    [DisplayName("شناسه")]
    public long Id { get; set; }
    
    [DisplayName("عنوان")]
    public string Title { get; set; } = string.Empty;

    public bool IsConfirmed { get; set; }
}
public record UpdateTagCommandResponse;
