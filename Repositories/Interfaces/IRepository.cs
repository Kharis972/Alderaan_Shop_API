namespace alderaan_shop.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetEntityByIdAsync<TKey>(TKey id);
    Task<IEnumerable<T>> GetAllEntitiesWithPagingAsync(int index, int entityNumber);
    Task SaveToDatabaseAsync(T entity);
    Task UpdateToDatabaseAsync(T entity);
}