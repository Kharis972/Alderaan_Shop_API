using System.Linq.Expressions;

namespace alderaan_shop.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetEntityByIdAsync<TKey>(TKey id);
    Task<IEnumerable<T>> GetAllEntitiesWithPagingAsync(int index, int entityNumber);
    Task SaveToDatabaseAsync(T entity);
    Task UpdateToDatabaseAsync(T entity);

    // Requêtes ciblées génériques
    Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate);
}