namespace AiTalentGenome.IdentityService.Application.DTOs;

public record HhProfileDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string? MiddleName,
    HhEmployerDto? Employer
);