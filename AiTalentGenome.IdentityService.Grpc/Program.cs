using AiTalentGenome.IdentityService.Application.DependencyInjection;
using AiTalentGenome.IdentityService.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.Run();