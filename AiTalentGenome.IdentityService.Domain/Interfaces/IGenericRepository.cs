using System.Linq.Expressions;

namespace AiTalentGenome.IdentityService.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    
    IQueryable<T> Find(Expression<Func<T, bool>> expression);
    
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Remove(T entity);
    
    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken ct = default);
}