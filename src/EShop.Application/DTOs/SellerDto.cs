namespace EShop.Application.DTOs;

public record SearchSellerDto : BaseSearchDto
{
    [DisplayName("مرتب کردن بر اساس")]
    public SortingSellerBy SortingBy { get; set; }

    public ActivationStatus ActivationStatus { get; set; }

    public string UserName { get; init; } = string.Empty;

    public string ShopName { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;
    
    public string Province { get; init; } = string.Empty;
}    


public record ShowSellerDto(
    long Id,
    long UserId,
    string UserName,
    bool IsLegal,
    string ShopName,
    string? Logo,
    string? WebSite,
    string City,
    string Province,
    DateTime CreatedDate,
    DocumentStatus DocumentStatus,
    ShowSellerDetailsDto? Details
);

public record ShowSellerDetailsDto(
    string? RejectReason,
    string PostalCode,
    string Address,
    string? NationalId,
    string? CartOrShebaNumber,
    string? AboutSeller,
    string? CompanyName,
    string? RegisterNumber,
    string? EconomicCode,
    string? SignatureOwners,
    CompanyType? CompanyType
);

public record IndividualSellerDto
{
    [DisplayName("کد ملی")]
    public string NationalId { get; set; }=string.Empty;
    
    [DisplayName("شماره کارت/شبا")]
    public string CartOrShebaNumber { get; set; }=string.Empty;
    
    [DisplayName("درباره فروشنده")]
    public string AboutSeller { get; set; }=string.Empty;
}

public record LegalSellerDto
{
    [DisplayName("نام شرکت")]
    public string CompanyName { get; set; }=string.Empty;
    
    [DisplayName("شماره ثبت")]
    public string RegisterNumber { get; set; }=string.Empty;
    
    [DisplayName("کد اقتصادی")]
    public string? EconomicCode { get; set; }=string.Empty;
    
    [DisplayName("صاحبان امضا")]
    public string SignatureOwners { get; set; }=string.Empty;
    
    [DisplayName("شماره شبا")]
    public string ShabaNumber { get; set; }=string.Empty;
    
    [DisplayName("نوع شرکت")]
    public CompanyType CompanyType { get; set; }
}
public enum SortingSellerBy
{
    Id,
    UserId,
    UserName,
    IsLegal,
    CreatedDate,
    DocumentStatus,
}