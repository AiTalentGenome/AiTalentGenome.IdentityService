namespace AiTalentGenome.IdentityService.Application.Features.HeadHunter.Common;

public record UserResult(
    long Id,
    string Email,
    string FirstName,
    string LastName,
    string CompanyName,
    string Position,
    bool IsActive,
    string? ErrorMessage = null
);