using System.Linq.Expressions;
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
            ArgumentNullException.ThrowIfNull(entity);
            
            try
            {
                Logger.LogDebug("Début de l'ajout de l'entité {EntityType} au DbSet", typeof(T).Name);
                await DbSet.AddAsync(entity);
                
                Logger.LogDebug("Début de la sauvegarde des changements en base de données");
                int changesSaved = await DbContext.SaveChangesAsync();
                
                Logger.LogInformation("Sauvegarde réussie. Nombre de changements: {ChangesCount}", changesSaved);
            }
            catch (DbUpdateException dbEx)
            {
                string detailedMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                string exceptionType = dbEx.InnerException?.GetType().Name ?? "Unknown";
                
                Logger.LogError(dbEx, 
                    "Erreur DbUpdateException. Type d'exception interne: {InnerExceptionType}, Message: {DetailedMessage}", 
                    exceptionType, 
                    detailedMessage);
                
                bool isUniqueConstraintViolation = detailedMessage.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) || 
                                                   detailedMessage.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
                                                   detailedMessage.Contains("IX_", StringComparison.OrdinalIgnoreCase);
                
                bool isForeignKeyViolation = detailedMessage.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
                                            detailedMessage.Contains("FK_", StringComparison.OrdinalIgnoreCase);
                
                bool isStringTruncation = detailedMessage.Contains("truncate", StringComparison.OrdinalIgnoreCase) ||
                                         detailedMessage.Contains("String or binary data would be truncated", StringComparison.OrdinalIgnoreCase);
                
                if (isUniqueConstraintViolation)
                {
                    Logger.LogWarning("Violation de contrainte d'unicité détectée");
                    throw new DatabaseException("Cette entité existe déjà en base de données.");
                }
                
                if (isForeignKeyViolation)
                {
                    Logger.LogWarning("Violation de clé étrangère détectée");
                    throw new DatabaseException("Une référence requise est manquante ou invalide.");
                }
                
                if (isStringTruncation)
                {
                    Logger.LogWarning("Données trop longues pour la colonne de base de données");
                    throw new DatabaseException("Les données fournies sont trop longues pour être enregistrées.");
                }
                
                throw new DatabaseException($"Erreur de base de données: {detailedMessage}");
            }
            catch (Exception e)
            {
                Logger.LogError(e, 
                    "Erreur inattendue lors de la sauvegarde. Type: {ExceptionType}, Message: {Message}", 
                    e.GetType().FullName, 
                    e.Message);
                
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

    // Requêtes ciblées génériques
    public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await DbSet.Where(predicate).ToListAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Erreur de liason avec la base de données (GetWhereAsync)!");
            throw new DatabaseException("Une erreur est survenue, veuillez contacter le service client !");
        }
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Erreur de liason avec la base de données (FirstOrDefaultAsync)!");
            throw new DatabaseException("Une erreur est survenue, veuillez contacter le service client !");
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await DbSet.AnyAsync(predicate);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Erreur de liason avec la base de données (AnyAsync)!");
            throw new DatabaseException("Une erreur est survenue, veuillez contacter le service client !");
        }
    }

    public async Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await DbSet.CountAsync(predicate);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Erreur de liason avec la base de données (CountWhereAsync)!");
            throw new DatabaseException("Une erreur est survenue, veuillez contacter le service client !");
        }
    }
}