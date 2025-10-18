using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;


/// Contrôleur REST pour les items de wishlist.
/// Route de base: api/WishlistItem
[ApiController]
[Route("api/[controller]")]
public class WishlistItemController : ControllerBase
{
    private readonly IWishlistItemService _wishlistItemService;
    private readonly ILogger<WishlistItemController> _logger;

    public WishlistItemController(IWishlistItemService wishlistItemService, ILogger<WishlistItemController> logger)
    {
        _wishlistItemService = wishlistItemService;
        _logger = logger;
    }
    
    /// Retourne les items d'une wishlist.
    [HttpGet("by-wishlist/{wishlistId:guid}")]
    public async Task<IActionResult> GetByWishlist(Guid wishlistId)
    {
        try
        {
            if (wishlistId == Guid.Empty) return BadRequest(new { Error = "Invalid wishlistId" });
            IEnumerable<WishlistItem> items = await _wishlistItemService.GetByWishlistIdAsync(wishlistId);
            return Ok(items);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetByWishlist");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetByWishlist");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Indique si un produit est déjà présent dans une wishlist.
    [HttpGet("exists")]
    public async Task<IActionResult> Exists([FromQuery] Guid wishlistId, [FromQuery] Guid productId)
    {
        try
        {
            if (wishlistId == Guid.Empty) return BadRequest(new { Error = "Invalid wishlistId" });
            if (productId == Guid.Empty) return BadRequest(new { Error = "Invalid productId" });
            bool present = await _wishlistItemService.ExistsAsync(wishlistId, productId);
            return Ok(new { Exists = present });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in Exists");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in Exists");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
}
