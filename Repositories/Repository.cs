using alderaan_shop.Data;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbSet<T> DbSet;

    protected readonly ApplicationDatabaseContext DbContext;

    protected ILogger<Repository<T>> Logger;

    public Repository(ApplicationDatabaseContext dbContext, ILogger<Repository<T>> logger)
    {
        DbContext = dbContext;
        Logger = logger;
        DbSet = DbContext.Set<T>();
    }

    public async Task<T?> GetEntityByIdAsync<TKey>(TKey id)
    {
        try
        {
            return await DbSet.FindAsync(id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Erreur de liason avec la base de données !");
            throw new DatabaseException("Une erreur est survenue, veuillez contacter le service client !");
        }
    }

    public async Task<IEnumerable<T>> GetAllEntitiesWithPagingAsync(int index, int entityNumber)
    {
        try
        {
            //skip le nombre de pages -1 et multiplie par le nombre d'entitées et renvoie un liste d'entitées
            return await DbSet.Skip(index * entityNumber).Take(entityNumber).ToListAsync();//(0 x 25) pour la 1er page et ainsi de suite
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Erreur de liason avec la base de données !");
            throw new DatabaseException("Une erreur est survenue, veuillez contacter le service client !");
        }
    }

    public async Task SaveToDatabaseAsync(T entity)
    {
        try
        {
            await DbSet.AddAsync(entity);
            //si il y a plus de 0 changements return true
            await DbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Erreur de liason avec la base de données !");
            throw new DatabaseException("Une erreur est survenue, veuillez contacter le service client !");
        }
    }

    public async Task UpdateToDatabaseAsync(T entity)
    {
        try
        {
            DbSet.Update(entity);
            await DbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Erreur de liason avec la base de données !");
            throw new DatabaseException("Une erreur est survenue, veuillez contacter le service client !");
        }
    }
}