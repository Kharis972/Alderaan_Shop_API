using System.Text.Json;
using alderaan_shop.DTOs.Product;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Helpers.Encryption;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service produit: applique la logique métier (validation, chiffrement/déchiffrement)
/// et délègue l'accès aux données au repository.
/// </summary>
public class ProductService : Service<Product>, IProductService
{
    private readonly IProductRepository _productRepository;
    
    /// <summary>
    /// Constructeur avec injection du repository produit et du logger.
    /// </summary>
    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger) : base(productRepository, logger)
    {
        _productRepository = productRepository;
        Logger = logger;
    }
    
    /// <summary>
    /// Récupère tous les produits depuis la base, puis déchiffre les champs sensibles et
    /// matérialise la collection en mémoire (ToList) pour éviter les ré-énumérations.
    /// </summary>
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        try
        {
            // Écrit un message d'information dans les logs pour indiquer le début de l'opération
            Logger.LogInformation("Getting all products");
        
            // Récupérer les produits chiffrés depuis la base de données
            IEnumerable<Product> encryptedProducts = await _productRepository.GetAllProductsAsync();
        
            // Décrypter les produits et matérialiser immédiatement la collection avec ToList()
            // ToList() force l'exécution du Select maintenant (une seule fois)
            // Résultat : List<Product> contenant tous les produits déchiffrés en mémoire
            List<Product> decryptedProducts = encryptedProducts.Select(p => {
            // Déchiffre le nom du produit et le stocke dans une variable string
            string decryptedName = Encryption.DecipherData(p.Name);
            
            // Déchiffre la description du produit et la stocke dans une variable string
            string decryptedDescription = Encryption.DecipherData(p.Description);
            
            // Déchiffre l'URL de l'image du produit et la stocke dans une variable string
            string decryptedImageUrl = Encryption.DecipherData(p.ImageUrl);
        
            // Crée et retourne une nouvelle instance de Product avec les données déchiffrées
            return new Product(
                p.Id,
                decryptedName.ToCharArray().AsMemory(),
                decryptedDescription.ToCharArray().AsMemory(),
                p.Price,
                decryptedImageUrl.ToCharArray().AsMemory(),
                p.Stock
            );
        }).ToList(); // ✅ ToList() exécute le Select MAINTENANT et stocke le résultat
    
        // Log le nombre de produits récupérés
        // Maintenant, Count est juste une propriété de la List, pas une nouvelle énumération
        Logger.LogInformation("Successfully retrieved and decrypted {Count} products", decryptedProducts.Count);
    
        // Retourne la liste des produits déchiffrés (déjà en mémoire, pas de ré-énumération)
        return decryptedProducts;
    }
    catch (Exception e)
    {
        // En cas d'erreur, log l'exception complète avec sa stack trace
        Logger.LogError(e, "Erreur lors de la récupération des produits");
        
        // Lance une exception personnalisée avec un message utilisateur
        throw new InternalServerException("Impossible de récupérer les produits.");
    }
}

    /// <summary>
    /// Ajoute un produit après validation des champs et chiffrement des données sensibles.
    /// </summary>
    public async Task<Product> AddProductAsync(NewProductDTO newProductDto)
    {
        try
        {
            Console.WriteLine("DTO reçu : " + JsonSerializer.Serialize(newProductDto));
            
            // Validation des données avant traitement
            if (newProductDto.NameMemoryChar.IsEmpty || newProductDto.NameMemoryChar.Length == 0)
            {
                throw new ArgumentException("Le nom du produit ne peut pas être vide.");
            }
        
            if (newProductDto.DescriptionMemoryChar.IsEmpty || newProductDto.DescriptionMemoryChar.Length == 0)
            {
                throw new ArgumentException("La description du produit ne peut pas être vide.");
            }
        
            if (newProductDto.ImageUrlMemoryChar.IsEmpty || newProductDto.ImageUrlMemoryChar.Length == 0)
            {
                throw new ArgumentException("L'URL de l'image ne peut pas être vide.");
            }
            
            Guid productId = Guid.NewGuid();
            
            Console.WriteLine("Nom : " + newProductDto.Name);
            Console.WriteLine("NomMemoryChar Length : " + newProductDto.NameMemoryChar.Length);

            Console.WriteLine("Description : " + newProductDto.Description);
            Console.WriteLine("DescriptionMemoryChar Length : " + newProductDto.DescriptionMemoryChar.Length);

            Console.WriteLine("ImageUrl : " + newProductDto.ImageUrl);
            Console.WriteLine("ImageUrlMemoryChar Length : " + newProductDto.ImageUrlMemoryChar.Length);

            // Convertir Memory<char> en char[] pour le chiffrement
            char[] nameArray = newProductDto.NameMemoryChar.ToArray();
            char[] descriptionArray = newProductDto.DescriptionMemoryChar.ToArray();
            char[] imageUrlArray = newProductDto.ImageUrlMemoryChar.ToArray();

            // Instancie le produit avec les données chiffrées
            Product product = new Product(
                productId,
                Encryption.CipherData(nameArray),
                Encryption.CipherData(descriptionArray),
                newProductDto.Price,
                Encryption.CipherData(imageUrlArray),
                newProductDto.Stock
            );

            // Nettoyer les tableaux sensibles après utilisation
            Array.Clear(nameArray, 0, nameArray.Length);
            Array.Clear(descriptionArray, 0, descriptionArray.Length);
            Array.Clear(imageUrlArray, 0, imageUrlArray.Length);

            await _productRepository.SaveToDatabaseAsync(product);
            return product;
        }
        catch (Exception e) when (e is ProductAlreadyExistsException or DatabaseException)
        {
            Console.WriteLine("Exception relancée : " + e.Message);
            throw;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Le produit n'a pas pu être ajouté. Détails: {ErrorMessage}", e.Message);
            // Préserver l'exception interne pour faciliter le débogage
            throw new InternalServerException("L'ajout du produit a échoué.");
        }
    }

    /// <summary>
    /// Met à jour un produit existant. Vérifie l'existence puis réécrit les champs chiffrés.
    /// </summary>
    public async Task<Product> EditProductAsync(Guid productId, EditProductDTO editProductDto)
    {
        try
        {
            // Récupérer le produit existant
            Product existingProduct = await _productRepository.CheckForProductByIdAsync(productId);
            if (existingProduct == null)
            {
                throw new ProductNotFoundException("Produit non trouvé.");
            }

            // Créer une nouvelle instance avec les données mises à jour
            Product updatedProduct = new Product(
                productId, // ID passé en paramètre
                Encryption.CipherData(editProductDto.NameMemoryChar),
                Encryption.CipherData(editProductDto.DescriptionMemoryChar),
                editProductDto.Price,
                Encryption.CipherData(editProductDto.ImageUrlMemoryChar),
                editProductDto.Stock
            );

            await _productRepository.UpdateProductAsync(updatedProduct);
            return updatedProduct;
        }
        catch (Exception e) when (e is ProductNotFoundException or DatabaseException)
        {
            Console.WriteLine("Exception relancée : " + e.Message);
            throw;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "L'édition du produit a échoué.");
            throw new InternalServerException("L'édition du produit a échoué.");
        }
    }
}