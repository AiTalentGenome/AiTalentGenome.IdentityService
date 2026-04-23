namespace AiTalentGenome.IdentityService.Domain.Entities;

public class Company
{
    public long Id { get; set; } // bigint в базе
    public string HhEmployerId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime SubscriptionExpiresAt { get; set; }
    
    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}