using AiTalentGenome.IdentityService.Application.DependencyInjection;
using AiTalentGenome.IdentityService.Grpc.Interceptors;
using AiTalentGenome.IdentityService.Grpc.Services;
using AiTalentGenome.IdentityService.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    // Добавляем наш перехватчик ошибок
    options.Interceptors.Add<ExceptionInterceptor>();
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.MapGrpcService<IdentityGrpcService>();

app.Run();