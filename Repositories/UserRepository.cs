using alderaan_shop.Data; 
using alderaan_shop.Models; 
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace alderaan_shop.Repositories;

// Définition de la classe UserRepository qui hérite de Repository<User> et implémente IUserRepository
/// <summary>
/// Repository concret pour l'entité User.
/// Fournit des requêtes ciblées autour de l'authentification, de la recherche par HMAC et des rôles.
/// </summary>
public class UserRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<User>> logger)
    : Repository<User>(dbContext, logger), IUserRepository
{
    /// <summary>
    /// Récupère un utilisateur par l'HMAC de son email.
    /// </summary>
    /// <param name="userMail">HMAC unique de l'adresse email.</param>
    /// <returns>Utilisateur correspondant ou null si introuvable.</returns>
    public async Task<User?> GetUserByMailHmacAsync(string userMail)
    {
           try
           {
               // Recherche du premier utilisateur dont le champ Mail correspond à userMail
               return await DbSet.FirstOrDefaultAsync(u => u.MailUniqueHMac == userMail);
           }
           catch (Exception e)
           {
               // Affichage de l'erreur en console et relance de l'exception
               Console.WriteLine(e);
               throw;
           }
       } 

    /// <summary>
    /// Retourne une liste paginée d'utilisateurs filtrés par rôle administrateur.
    /// </summary>
    /// <param name="isAdmin">True pour administrateurs, false pour utilisateurs standards.</param>
    /// <param name="index">Index de page (0-based).</param>
    /// <param name="entityNumber">Taille de page.</param>
    public async Task<IEnumerable<User>> GetUsersByRoleWithPagingAsync(bool isAdmin, int index, int entityNumber)
    {
        try
        {
            // Pagination : saute les éléments précédents et prend un nombre défini d'utilisateurs
            // Filtrage par rôle (IsAdmin)
            return await DbSet
                .Skip(index * entityNumber)
                .Take(entityNumber)
                .Where(u => u.IsAdmin == isAdmin)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Vérifie si un utilisateur existe via ses index uniques (HMAC du téléphone ou de l'email).
    /// </summary>
    /// <param name="phoneNumberUniqueHmac">HMAC unique du numéro de téléphone.</param>
    /// <param name="mailUniqueHmac">HMAC unique de l'adresse email.</param>
    /// <returns>True si un utilisateur existe avec l'un des deux HMAC, sinon false.</returns>
    public async Task<bool> CheckForUserByUniqueIndexesAsync(string phoneNumberUniqueHmac, string mailUniqueHmac)
    {
        try
        {
            // Vérifie si un utilisateur existe avec l'un ou l'autre des HMAC fournis
            return await DbSet.AnyAsync(u =>
                u.PhoneNumberUniqueHMac == phoneNumberUniqueHmac ||
                u.MailUniqueHMac == mailUniqueHmac);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Vérifie si un utilisateur donné par son ID possède le rôle administrateur.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur.</param>
    /// <returns>True si l'utilisateur est admin, sinon false.</returns>
    public async Task<bool> CheckForUserRoleByIdAsync(Guid id)
    {
        try
        {
            // Vérifie si un utilisateur avec l'ID donné est un admin
            return await DbSet.AnyAsync(u => u.Id == id && u.IsAdmin == true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Vérifie l'existence d'un utilisateur par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'utilisateur.</param>
    /// <returns>True si l'utilisateur existe, sinon false.</returns>
    public async Task<bool> CheckForUserByIdAsync(Guid id)
    {
        try
        {
            // Vérifie si un id existe 
            return await DbSet.AnyAsync(u => u.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}