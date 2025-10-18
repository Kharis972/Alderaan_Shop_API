using alderaan_shop.Data;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Product.
/// Fournit des opérations spécifiques liées aux produits en plus du CRUD générique.
/// </summary>
public class ProductRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Product>> logger)
: Repository<Product>(dbContext, logger), IProductRepository
{
    /// <summary>
    /// Vérifie l'existence d'un produit et le retourne par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique du produit.</param>
    /// <returns>Le produit correspondant si trouvé; lève une exception en cas d'erreur base.</returns>
    public async Task<Product> CheckForProductByIdAsync(Guid id)
    {
        try
        {
            return await DbSet.FindAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Récupère l'ensemble des produits tels qu'enregistrés en base (chiffrés si applicable côté modèle/service).
    /// </summary>
    /// <returns>Une liste de produits.</returns>
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        try
        {
            return await DbSet.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Met à jour un produit existant.
    /// </summary>
    /// <param name="product">Produit avec ses champs à jour.</param>
    /// <returns>Le produit mis à jour.</returns>
    public async Task<Product> UpdateProductAsync(Product product)
    {
        try
        {
            DbSet.Update(product);
            await dbContext.SaveChangesAsync();
            return product;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error updating product with ID {product?.Id}: {e}");
            throw new DatabaseException("Failed to update product in database.");
        }
    }
}