using Newtonsoft.Json;

namespace EShop.Application.DTOs;

public class PaymentDto
{
    [JsonProperty("MerchantID")]
    public string MerchantId { get; set; }=string.Empty;

    [JsonProperty("Amount")]
    public int Amount { get; set; }

    [JsonProperty("Description")]
    public string Description { get; set; }=string.Empty;

    [JsonProperty("Email")]
    public string Email { get; set; }=string.Empty;

    [JsonProperty("Mobile")]
    public string Mobile { get; set; }=string.Empty;

    [JsonProperty("CallbackURL")]
    public string CallbackUrl { get; set; }=string.Empty;
}
public record VerifyPaymentDto 
{
    public string MerchantId { get; set; }= string.Empty;
    
    public string Authority { get; set; }= string.Empty;
    
    public long Amount { get; set; }
}