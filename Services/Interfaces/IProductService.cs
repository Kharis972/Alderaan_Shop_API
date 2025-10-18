using alderaan_shop.DTOs.Product;
using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Contrat du service applicatif pour la gestion des produits.
/// Encapsule la logique métier (chiffrement/déchiffrement, validations) et délègue l'accès aux données au repository.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Récupère et déchiffre l'ensemble des produits.
    /// </summary>
    Task<IEnumerable<Product>> GetAllProductsAsync();

    /// <summary>
    /// Ajoute un nouveau produit après validation et chiffrement.
    /// </summary>
    Task<Product> AddProductAsync(NewProductDTO newProductDto);

    /// <summary>
    /// Met à jour un produit existant en réécrivant les champs chiffrés.
    /// </summary>
    Task<Product> EditProductAsync(Guid productId, EditProductDTO editProductDto);
}