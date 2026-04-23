namespace AiTalentGenome.IdentityService.Application.Features.HeadHunter.Common;

public record AuthResult(
    long UserId, // Наш новый числовой ID
    string AccessToken,
    bool IsActive,
    string? ErrorMessage = null
);