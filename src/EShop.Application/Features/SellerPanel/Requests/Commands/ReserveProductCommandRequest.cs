using System.Text.Json.Serialization;

namespace EShop.Application.Features.SellerPanel.Requests.Commands;

public record ReserveProductCommandRequest : IRequest<ReserveProductCommandResponse>
{
    [DisplayName("شناسه محصول")]
    public long ProductId { get; set; }
    
    [DisplayName("شماسه فروشنده")]
    [JsonIgnore] public long SellerId { get; set; }
    
    [DisplayName("رنگ")]
    public string ColorCode { get; set; } = string.Empty;
    
    [DisplayName("قیمت")]
    public uint BasePrice { get; set; }
    
    [DisplayName("تعداد")]
    public short Count { get; set; }
    
    [DisplayName("تخفیف")]
    public byte DiscountPercentage { get; set; }
    
    [DisplayName("مدت زمان تخفیف")]
    public DateTime? EndOfDiscount { get; set; }
    
}

public record ReserveProductCommandResponse;