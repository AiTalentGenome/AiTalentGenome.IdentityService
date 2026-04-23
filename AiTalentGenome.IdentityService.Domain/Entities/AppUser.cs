namespace AiTalentGenome.IdentityService.Domain.Entities;

public class AppUser
{
    public long Id { get; set; }
    public string HhUserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Position { get; set; } = "Менеджер";

    public long CompanyId { get; set; }
    public Company Company { get; set; } = null!;
}