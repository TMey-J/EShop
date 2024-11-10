using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EShop.Application.Features.AdminPanel.Requests.Commands.Category;

public record CreateCategoryCommandRequest:IRequest<CreateCategoryCommandResponse>
{
    [DisplayName("عنوان")]
    public string Title { get; set; } = string.Empty;
    [DisplayName("والد")]
    public long? Parent { get; set; }
    [DisplayName("تصویر")]
    public string? PictureBase64 { get; set; }
}
public record CreateCategoryCommandResponse;
