namespace AiTalentGenome.IdentityService.Application.Features.HeadHunter.Common;

public record AuthResult(
    long UserId,
    string AccessToken,
    bool IsActive,
    string Email = "",
    string FirstName = "",
    string LastName = "",
    string CompanyName = "",
    string? ErrorMessage = null
);