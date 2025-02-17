using System.Text.Json.Serialization;

namespace EShop.Application.Features.AdminPanel.Product.Requests.Commands;

public record UpdateProductCommandRequest : IRequest<UpdateProductCommandResponse>
{
    [JsonIgnore]
    [DisplayName("شناسه")]
    public long Id { get; set; }
    
    [DisplayName("عنوان فارسی")]
    public string Title { get; set; } = string.Empty;

    [DisplayName("عنوان انگلیسی")]
    public string EnglishTitle { get; set; } = string.Empty;

    [DisplayName("توضیحات")]
    public string Description { get; set; } = string.Empty;

    [DisplayName("دسته بندی")]
    public long CategoryId { get; set; }
    
    [DisplayName("تگ ها")]
    public List<string> Tags { get; set; } = [];
    [DisplayName("تضاویر")]
    public List<string> Images { get; set; } = [];
    
    [DisplayName("ویژگی ها")]
    public Dictionary<string, string> Features { get; set; } = [];
}

public record UpdateProductCommandResponse();