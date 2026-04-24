// AiTalentGenome.IdentityService.Api/Services/IdentityGrpcService.cs

using AiTalentGenome.Contracts.Identity;
using AiTalentGenome.IdentityService.Application.Features.HeadHunter.Commands;
using AiTalentGenome.IdentityService.Application.Features.HeadHunter.Queries;
using Grpc.Core;
using MediatR;

namespace AiTalentGenome.IdentityService.Grpc.Services;

public class IdentityGrpcService(IMediator mediator) : Contracts.Identity.IdentityService.IdentityServiceBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<AuthResponse> ExchangeHhCode(
        ExchangeHhCodeRequest request, 
        ServerCallContext context)
    {
        var result = await _mediator.Send(new ExchangeHhCodeCommand(request.Code));

        if (!result.IsActive)
        {
            return new AuthResponse { IsActive = false, ErrorMessage = result.ErrorMessage };
        }

        return new AuthResponse
        {
            AccessToken = result.AccessToken,
            IsActive = true,
            User = new UserResponse
            {
                Id = result.UserId,
                Email = result.Email,
                FirstName = result.FirstName,
                LastName = result.LastName,
                CompanyName = result.CompanyName,
                IsActive = true
            }
        };
    }
    
    // AiTalentGenome.IdentityService.Api/Services/IdentityGrpcService.cs
    public override async Task<UserResponse> GetUserInfo(
        GetUserInfoRequest request, 
        ServerCallContext context)
    {
        var result = await _mediator.Send(new GetUserInfoQuery(request.AccessToken));

        if (!result.IsActive)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, result.ErrorMessage ?? "Сессия невалидна"));
        }

        return new UserResponse
        {
            Id = result.Id,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName,
            CompanyName = result.CompanyName,
            Position = result.Position,
            IsActive = result.IsActive
        };
    }

    public override async Task<ValidateSessionResponse> ValidateSession(
        ValidateSessionRequest request, 
        ServerCallContext context)
    {
        // В будущем здесь будет логика проверки JWT или токена в БД
        return await Task.FromResult(new ValidateSessionResponse { IsValid = true });
    }
}