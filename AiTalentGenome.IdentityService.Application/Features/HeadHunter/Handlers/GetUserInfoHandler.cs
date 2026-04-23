using AiTalentGenome.IdentityService.Application.Features.HeadHunter.Common;
using AiTalentGenome.IdentityService.Application.Features.HeadHunter.Queries;
using AiTalentGenome.IdentityService.Application.Interfaces;
using AiTalentGenome.IdentityService.Domain.Entities;
using AiTalentGenome.IdentityService.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTalentGenome.IdentityService.Application.Features.HeadHunter.Handlers;

// Application/Authentication/Handlers/GetUserInfoHandler.cs
public class GetUserInfoHandler(
    IHeadHunterProvider hhProvider,
    IGenericRepository<AppUser> userRepo
) : IRequestHandler<GetUserInfoQuery, UserResult>
{
    public async Task<UserResult> Handle(GetUserInfoQuery request, CancellationToken ct)
    {
        try 
        {
            // 1. Спрашиваем у HH, чей это токен
            var hhProfile = await hhProvider.GetUserProfileAsync(request.AccessToken, ct);

            // 2. Ищем пользователя в нашей БД по его HhUserId
            var user = await userRepo.Find(u => u.HhUserId == hhProfile.Id)
                .Include(u => u.Company) // Подгружаем данные компании (Join)
                .FirstOrDefaultAsync(ct);

            if (user == null)
                return new UserResult(0, "", "", "", "", "", false, "Пользователь не найден в локальной БД");

            return new UserResult(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Company.Name,
                user.Position,
                user.Company.IsActive
            );
        }
        catch (Exception ex)
        {
            return new UserResult(0, "", "", "", "", "", false, ex.Message);
        }
    }
}