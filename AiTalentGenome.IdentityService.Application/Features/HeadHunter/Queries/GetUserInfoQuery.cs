using AiTalentGenome.IdentityService.Application.Features.HeadHunter.Common;
using MediatR;

namespace AiTalentGenome.IdentityService.Application.Features.HeadHunter.Queries;

public record GetUserInfoQuery(string AccessToken) : IRequest<UserResult>;