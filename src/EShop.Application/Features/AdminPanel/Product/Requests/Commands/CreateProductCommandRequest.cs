using System.Text.Json.Serialization;

namespace EShop.Application.Features.AdminPanel.Product.Requests.Commands;

public record CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
{
    [DisplayName("عنوان فارسی")]
    public string Title { get; set; } = string.Empty;

    [DisplayName("عنوان انگلیسی")]
    public string EnglishTitle { get; set; } = string.Empty;

    [DisplayName("قیمت")]
    public uint BasePrice { get; set; }

    [DisplayName("تعداد")]
    public int Count { get; set; }

    [DisplayName("توضیحات")]
    public string Description { get; set; } = string.Empty;

    [DisplayName("درصد تخفیف")]
    public byte DiscountPercentage { get; set; }
    
    [DisplayName("زمان پایان تخفیف")]
    public DateTime? EndOfDiscount { get; set; }

    [DisplayName("دسته بندی")]
    public long CategoryId { get; set; }
    
    [DisplayName("رنگ ها")]
    public List<string> ColorsCode { get; set; } = [];
    
    [DisplayName("تگ ها")]
    public List<string> Tags { get; set; } = [];
    [DisplayName("تضاویر")]
    public List<string> Images { get; set; } = [];

    [JsonIgnore]
    public long SellerId { get; set; }
}

public record CreateProductCommandResponse();