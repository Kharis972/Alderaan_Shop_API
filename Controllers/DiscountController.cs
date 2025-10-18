using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour les réductions.
/// Route de base: api/Discount
[ApiController]
[Route("api/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;
    private readonly ILogger<DiscountController> _logger;

    public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger)
    {
        _discountService = discountService;
        _logger = logger;
    }
    
    /// Liste des réductions actives.
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            IEnumerable<Discount> discounts = await _discountService.GetActiveAsync();
            return Ok(discounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetActive");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Réductions valides maintenant (UTC).
    [HttpGet("valid-now")]
    public async Task<IActionResult> GetValidNow()
    {
        try
        {
            IEnumerable<Discount> discounts = await _discountService.GetValidNowAsync(DateTime.UtcNow);
            return Ok(discounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetValidNow");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Réductions d'un produit.
    [HttpGet("by-product/{productId:guid}")]
    public async Task<IActionResult> GetForProduct(Guid productId)
    {
        try
        {
            if (productId == Guid.Empty) return BadRequest(new { Error = "Invalid productId" });
            IEnumerable<Discount> discounts = await _discountService.GetForProductAsync(productId);
            return Ok(discounts);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetForProduct");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetForProduct");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Réductions d'une catégorie.
    [HttpGet("by-category/{categoryId:guid}")]
    public async Task<IActionResult> GetForCategory(Guid categoryId)
    {
        try
        {
            if (categoryId == Guid.Empty) return BadRequest(new { Error = "Invalid categoryId" });
            IEnumerable<Discount> discounts = await _discountService.GetForCategoryAsync(categoryId);
            return Ok(discounts);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetForCategory");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetForCategory");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Réductions globales (ni produit, ni catégorie).
    [HttpGet("global")]
    public async Task<IActionResult> GetGlobal()
    {
        try
        {
            IEnumerable<Discount> discounts = await _discountService.GetGlobalAsync();
            return Ok(discounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetGlobal");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
}
