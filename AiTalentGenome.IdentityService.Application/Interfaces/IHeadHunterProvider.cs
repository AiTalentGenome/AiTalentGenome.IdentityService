using AiTalentGenome.IdentityService.Application.DTOs;

namespace AiTalentGenome.IdentityService.Application.Interfaces;

public interface IHeadHunterProvider
{
    Task<string> ExchangeCodeForTokenAsync(string code, CancellationToken ct = default);

    Task<HhProfileDto> GetUserProfileAsync(string accessToken, CancellationToken ct = default);
}