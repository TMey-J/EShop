namespace EShop.Application.DTOs;

public record ShowOrderDetailsDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public uint BasePrice { get; set; }
    public short Count { get; set; }
    public byte DiscountPercentage { get; set; }
    public DateTime? EndOfDiscount { get; set; }
    public uint PriceWithDiscount { get; set; }
    public string Image { get; set; }=string.Empty;
    public string ColorName { get; set; }
    public string ColorCode { get; set; }
}