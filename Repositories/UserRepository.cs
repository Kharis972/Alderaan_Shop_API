using alderaan_shop.Data; 
using alderaan_shop.Models; 
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace alderaan_shop.Repositories;

// Définition de la classe UserRepository qui hérite de Repository<User> et implémente IUserRepository
public class UserRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<User>> logger)
    : Repository<User>(dbContext, logger), IUserRepository
{
    // Méthode pour récupérer un utilisateur par son adresse mail (HMAC)
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

    // Méthode pour récupérer une liste paginée d'utilisateurs selon leur rôle (admin ou non)
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

    // Méthode pour vérifier si un utilisateur existe via ses index uniques (HMAC du téléphone ou du mail)
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

    // Méthode pour vérifier si un utilisateur donné par son ID est un administrateur
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