namespace alderaan_shop.Services.Interfaces;

public interface IService<T> where T : class
{
    Task<T?> GetEntityByIdAsync<TKey>(TKey id) where TKey : notnull;
    Task<IEnumerable<T>> GetAllEntitiesWithPagingAsync(int index, int entityNumber);
}