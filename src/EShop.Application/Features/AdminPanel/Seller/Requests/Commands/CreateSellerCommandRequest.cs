using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;

namespace EShop.Application.Features.AdminPanel.Seller.Requests.Commands;

public record CreateSellerCommandRequest : IRequest<CreateSellerCommandResponse>
{
    [DisplayName("شناسه کاربر")]
    public long UserId { get; set; }
    
    [DisplayName("سخص حقوقی/حقیقی")]
    public bool IsLegalPerson { get; set; }
    
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

public record CreateSellerCommandResponse;