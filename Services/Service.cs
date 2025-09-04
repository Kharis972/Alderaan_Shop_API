using alderaan_shop.Data;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Repositories.Interfaces;

namespace alderaan_shop.Services;

public class Service<T> where  T : class
{
    protected IRepository<T> _repository;
    protected ILogger<Service<T>> Logger;

    public Service(IRepository<T> repository, ILogger<Service<T>> logger)
    {
        _repository = repository;
        Logger = logger;
    } 

    public async Task<T?> GetEntityByIdAsync<TKey>(TKey id) where TKey : notnull
    {
        try
        {
            if (id is int or Guid)
                return await _repository.GetEntityByIdAsync(id); 
            throw new BadSqlRequestException("Identifiant Unique invalide !");
        }
        catch (Exception e) when (e is DatabaseException or BadSqlRequestException)
        {
            throw; 
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Une erreur est survenue !");
            throw new InternalServerException("Une erreur interne est survenue !");
        }
    } 

    public async Task<IEnumerable<T>> GetAllEntitiesWithPagingAsync(int index, int entityNumber)
    {
        try
        {
            if (index >= 0 && entityNumber >= 2)
                return await _repository.GetAllEntitiesWithPagingAsync(index, entityNumber);
            throw (new BadSqlRequestException("La portée du message est incorecte."));
        }
        catch (Exception e) when (e is DatabaseException or BadSqlRequestException)
        {
            throw;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Une erreur est survenue !");
            throw new InternalServerException("Une erreur interne est survenue !");
        }
    }
}