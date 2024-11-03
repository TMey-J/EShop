using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EShop.Application.Features.AdminPanel.Requests.Commands.Category;

public record CreateCategoryCommandRequest:IRequest<CreateCategoryCommandResponse>
{
    public string Title { get; set; } = string.Empty;
    public long? Parent { get; set; }
    public string? PictureBase64 { get; set; }
}
public record CreateCategoryCommandResponse;
