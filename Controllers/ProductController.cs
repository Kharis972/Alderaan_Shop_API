using alderaan_shop.DTOs.Product;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Helpers.Encryption;
using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour la gestion des produits.
/// - Route de base: api/Product
/// - Expose des endpoints pour créer, modifier et lister les produits.
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    /// Constructeur avec injection du service produit et du logger.
    /// <param name="productService">Service applicatif responsable de la logique métier produit.</param>
    /// <param name="logger">Logger typé pour tracer les opérations du contrôleur.</param>
    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// Crée un nouveau produit.
    /// <param name="newProductDto">Données du produit à créer.</param>
    /// <returns>
    /// 200 OK si créé avec succès, 409 Conflict si le produit existe déjà,
    /// 500 en cas d'erreur inattendue.
    /// </returns>
    /// <response code="200">Produit créé.</response>
    /// <response code="409">Conflit: produit déjà existant.</response>
    /// <response code="500">Erreur interne.</response>
    [HttpPost("addProduct")]
    public async Task<IActionResult> AddProduct([FromBody] NewProductDTO newProductDto)
    {
        try
        {
            newProductDto.InitializeMemory();
            await _productService.AddProductAsync(newProductDto);
            
            Console.WriteLine("AddProductAsync called");
            return Ok(new
            {
                Message = "Product successfully created."
            });
        }
        catch (ProductAlreadyExistsException ex)
        {
            _logger.LogWarning($"AddProduct failed for name: {newProductDto.Name}. Reason: {ex.Message}");
            return Conflict("Unable to create product. Please try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Product name: HMAC name: " + Encryption.ComputeUniqueHmac(newProductDto.NameMemoryChar));
            Console.WriteLine("Error in AddProductAsync : " + ex);
            _logger.LogError(ex, "Unexpected error during addProduct.");
            return StatusCode(500, "An error occurred. Please try again later.");
        }
    }
    
    /// Met à jour un produit existant.
    /// <param name="id">Identifiant du produit à modifier.</param>
    /// <param name="editProductDto">Nouvelles valeurs du produit.</param>
    /// <returns>
    /// 200 OK si la mise à jour réussit, 404 si non trouvé, 500 si erreur.
    /// </returns>
    /// <response code="200">Produit mis à jour.</response>
    /// <response code="404">Produit introuvable.</response>
    /// <response code="500">Erreur interne.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> EditProduct(Guid id, [FromBody] EditProductDTO editProductDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Appelle le service sans stocker le résultat
            await _productService.EditProductAsync(id, editProductDto);
        
            // Log de succès avec l'ID du produit
            _logger.LogInformation($"Product successfully updated with ID: {id}");
        
            // Retourne seulement un message de succès
            return Ok(new { 
                Message = "Product successfully updated.", 
                ProductId = id 
            });
        }
        catch (ProductNotFoundException ex)
        {
            _logger.LogWarning($"EditProduct failed for ID: {id}. Reason: {ex.Message}");
            return NotFound(new { Error = "Product not found." });
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Database error during editProduct.");
            return StatusCode(500, new { Error = "Database error occurred. Please try again later." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during editProduct for ID: {ProductId}", id);
            return StatusCode(500, new { Error = "An unexpected error occurred. Please try again later." });
        }
    }


    /// Récupère la liste de tous les produits (déchiffrés côté service).
    /// <returns>200 OK avec la liste des produits ou 500 en cas d'erreur.</returns>
    /// <response code="200">Liste des produits retournée.</response>
    /// <response code="500">Erreur interne.</response>
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            IEnumerable<Product> products = await _productService.GetAllProductsAsync();
            
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            return StatusCode(500, new { Error = "An error occurred while retrieving products." });
        }
    }
}