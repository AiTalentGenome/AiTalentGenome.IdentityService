namespace AiTalentGenome.IdentityService.Infrastructure.Options;

public class HhOptions
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
    public string AppName { get; set; } = string.Empty;
}