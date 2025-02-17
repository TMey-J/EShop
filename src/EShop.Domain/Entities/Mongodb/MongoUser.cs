namespace EShop.Domain.Entities.Mongodb;

public class MongoUser:MongoBaseEntity
{
    public string? PhoneNumber { get; set; }
    public string UserName { get; set; }=string.Empty;
    public DateTime SendCodeLastTime { get; set; }
    public bool IsActive { get; set; }
    public string? Avatar { get; set; }
    public byte[] PasswordSalt { get; set; } = [];
    public string? NormalizedUserName { get; set; }
    public string? Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }=string.Empty;
    public string? SecurityStamp { get; set; }
    public string? ConcurrencyStamp { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }
}