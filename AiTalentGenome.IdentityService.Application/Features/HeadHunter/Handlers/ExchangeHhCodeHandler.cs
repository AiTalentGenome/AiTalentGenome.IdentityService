// AiTalentGenome.IdentityService.Application/Authentication/Handlers/ExchangeHhCodeHandler.cs

using AiTalentGenome.IdentityService.Application.Features.HeadHunter.Commands;
using AiTalentGenome.IdentityService.Application.Features.HeadHunter.Common;
using AiTalentGenome.IdentityService.Application.Interfaces;
using AiTalentGenome.IdentityService.Domain.Entities;
using AiTalentGenome.IdentityService.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTalentGenome.IdentityService.Application.Features.HeadHunter.Handlers;

public class ExchangeHhCodeHandler(
    IHeadHunterProvider hhProvider,
    IGenericRepository<Company> companyRepo,
    IGenericRepository<AppUser> userRepo,
    IUnitOfWork unitOfWork
) : IRequestHandler<ExchangeHhCodeCommand, AuthResult>
{
    public async Task<AuthResult> Handle(ExchangeHhCodeCommand request, CancellationToken ct)
    {
        // 1. Получаем токен от HH
        var accessToken = await hhProvider.ExchangeCodeForTokenAsync(request.Code, ct);
        
        // 2. Получаем профиль пользователя
        var profile = await hhProvider.GetUserProfileAsync(accessToken, ct);

        if (profile.Employer == null)
            return new AuthResult(0, "", false, "Аккаунт не привязан к организации.");

        // 3. Работаем с компанией (используем наш Generic Repo)
        var company = await companyRepo.Find(c => c.HhEmployerId == profile.Employer.Id)
                                       .FirstOrDefaultAsync(ct);

        if (company == null)
        {
            company = new Company 
            { 
                HhEmployerId = profile.Employer.Id, 
                Name = profile.Employer.Name, 
                IsActive = true,
                SubscriptionExpiresAt = DateTime.UtcNow.AddDays(14) // Пробный период
            };
            await companyRepo.AddAsync(company, ct);
            // Сохраняем, чтобы получить ID компании для пользователя
            await unitOfWork.SaveChangesAsync(ct);
        }

        // 4. Работаем с пользователем
        var user = await userRepo.Find(u => u.HhUserId == profile.Id)
                                 .FirstOrDefaultAsync(ct);

        if (user == null)
        {
            user = new AppUser
            {
                HhUserId = profile.Id,
                Email = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                MiddleName = profile.MiddleName,
                CompanyId = company.Id // Используем long ID
            };
            await userRepo.AddAsync(user, ct);
        }
        else
        {
            // Обновляем данные, если они изменились в HH
            user.Email = profile.Email;
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
        }

        // 5. Финальное сохранение
        await unitOfWork.SaveChangesAsync(ct);

        return new AuthResult(user.Id, accessToken, company.IsActive);
    }
}