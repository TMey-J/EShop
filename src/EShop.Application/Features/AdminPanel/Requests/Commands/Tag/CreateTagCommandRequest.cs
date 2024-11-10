namespace EShop.Application.Features.AdminPanel.Requests.Commands.Tag;

public record CreateTagCommandRequest:IRequest<CreateTagCommandResponse>
{
    [DisplayName("عنوان")]
    public string Title { get; set; } = string.Empty;
}
public record CreateTagCommandResponse;
