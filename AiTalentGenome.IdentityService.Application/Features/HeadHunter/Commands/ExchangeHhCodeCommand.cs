using AiTalentGenome.IdentityService.Application.Features.HeadHunter.Common;
using MediatR;

namespace AiTalentGenome.IdentityService.Application.Features.HeadHunter.Commands;

public record ExchangeHhCodeCommand(string Code) : IRequest<AuthResult>;