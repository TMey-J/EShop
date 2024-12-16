namespace EShop.Application.Features.AdminPanel.Seller.Requests.Commands;

public record UpdateSellerCommandRequest : IRequest<UpdateSellerCommandResponse>
{
    [DisplayName("شناسه فروشنده")]
    public long SellerId { get; set; }
    
    [DisplayName("نام فروشگاه")]
    public string ShopName { get; set; } = string.Empty;
    
    [DisplayName("لگو")]
    public string? Logo { get; set; }
    
    [DisplayName("وب سایت")]
    public string? Website { get; set; }
    
    [DisplayName("شهر")]
    public long CityId { get; set; }
    
    [DisplayName("کد پستی")]
    public string PostalCode { get; set; } = string.Empty;
    
    [DisplayName("آدرس")]
    public string Address { get; set; } = string.Empty;

    public IndividualSellerDto? IndividualSeller { get; set; }
    public LegalSellerDto? LegalSeller { get; set; }
}

public record UpdateSellerCommandResponse;