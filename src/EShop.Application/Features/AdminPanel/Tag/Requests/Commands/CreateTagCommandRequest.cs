namespace EShop.Application.Features.AdminPanel.Tag.Requests.Commands;

public record CreateTagCommandRequest:IRequest<CreateTagCommandResponse>
{
    [DisplayName("عنوان")]
    public string Title { get; set; } = string.Empty;
}
public record CreateTagCommandResponse;
