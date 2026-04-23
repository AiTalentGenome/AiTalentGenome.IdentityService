using AiTalentGenome.IdentityService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AiTalentGenome.IdentityService.Domain.Interfaces;

namespace AiTalentGenome.IdentityService.Infrastructure.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(long id, CancellationToken ct = default) 
        => await _dbSet.FindAsync([id], ct);

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default) 
        => await _dbSet.ToListAsync(ct);

    public IQueryable<T> Find(Expression<Func<T, bool>> expression) 
        => _dbSet.Where(expression);

    public async Task AddAsync(T entity, CancellationToken ct = default) 
        => await _dbSet.AddAsync(entity, ct);

    public void Update(T entity) 
        => _dbSet.Update(entity);

    public void Remove(T entity) 
        => _dbSet.Remove(entity);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default) 
        => await _dbSet.AnyAsync(expression, ct);
}