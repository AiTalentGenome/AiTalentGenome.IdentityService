using AiTalentGenome.IdentityService.Application.Interfaces;
using AiTalentGenome.IdentityService.Domain.Interfaces;
using AiTalentGenome.IdentityService.Infrastructure.Data;
using AiTalentGenome.IdentityService.Infrastructure.Options;
using AiTalentGenome.IdentityService.Infrastructure.Repositories;
using AiTalentGenome.IdentityService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiTalentGenome.IdentityService.Infrastructure.DependencyInjection;

public static class InfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        // 2. Настройки (теперь должно работать после установки пакета)
        services.Configure<HhOptions>(configuration.GetSection("HeadHunter"));

        // 3. HTTP Клиент (теперь должно работать после установки пакета)
        services.AddHttpClient<IHeadHunterProvider, HeadHunterProvider>(client =>
        {
            client.BaseAddress = new Uri("https://api.hh.ru/");
        });

        // 4. Репозитории и UnitOfWork
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>(); // Добавляем регистрацию UoW
    }
}