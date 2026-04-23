using AiTalentGenome.IdentityService.Domain.Interfaces;
using AiTalentGenome.IdentityService.Infrastructure.Data;

namespace AiTalentGenome.IdentityService.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // EF Core автоматически оборачивает SaveChangesAsync в транзакцию
        return await _context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}