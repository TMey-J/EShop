namespace EShop.Application.Features.Comment.Requests.Command;

public record CreateCommentCommandRequest : IRequest<CreateCommentCommandResponse>
{
    [DisplayName("شناسه محصول")] public long ProductId { get; set; }

    [DisplayName("بدنه")] public string Body { get; set; } = string.Empty;

    [DisplayName("امتیاز")] public byte Rating { get; set; }

    public long? ReplayId { get; set; }
}

public record CreateCommentCommandResponse;