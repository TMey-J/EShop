namespace EShop.Application.Features.AdminPanel.Category.Requests.Commands;

public record AddFeaturesToCategoryCommandRequest:IRequest<AddFeaturesToCategoryCommandResponse>
{
    [DisplayName("شناسه دسته بندی")]
    public long CategoryId { get; set; }

    [DisplayName("ویژگی ها")]
    public List<string> FeaturesName { get; set; } = [];
}
public record AddFeaturesToCategoryCommandResponse;