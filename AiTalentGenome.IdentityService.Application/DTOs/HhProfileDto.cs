using System.Text.Json.Serialization;

namespace AiTalentGenome.IdentityService.Application.DTOs;

public record HhProfileDto(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("first_name")] string FirstName, // Важно!
    [property: JsonPropertyName("last_name")] string LastName,   // Важно!
    [property: JsonPropertyName("middle_name")] string? MiddleName,
    [property: JsonPropertyName("employer")] HhEmployerDto? Employer
);