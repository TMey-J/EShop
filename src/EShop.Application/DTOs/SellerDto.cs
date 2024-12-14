namespace EShop.Application.DTOs;

public record SearchSellerDto : BaseSearchDto
{
    [DisplayName("مرتب کردن بر اساس")]
    public SortingSellerBy SortingBy { get; set; }

    public ActivationStatus ActivationStatus { get; set; }

    public string UserName { get; init; } = string.Empty;

    public string ShopName { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;
}

public record ShowSellerDto(
    long Id,
    long UserId,
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

public enum SortingSellerBy
{
    Id,
    UserId,
    UserName,
    IsLegal,
    CreatedDate,
    DocumentStatus,
}