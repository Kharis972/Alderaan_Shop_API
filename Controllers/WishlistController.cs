using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour les wishlists.
/// Route de base: api/Wishlist
[ApiController]
[Route("api/[controller]")]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _wishlistService;
    private readonly ILogger<WishlistController> _logger;

    public WishlistController(IWishlistService wishlistService, ILogger<WishlistController> logger)
    {
        _wishlistService = wishlistService;
        _logger = logger;
    }
    
    /// Récupère la wishlist d'un utilisateur.
    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty) return BadRequest(new { Error = "Invalid userId" });
            Wishlist? wishlist = await _wishlistService.GetByUserIdAsync(userId);
            return wishlist != null ? Ok(wishlist) : NotFound(new { Error = "Wishlist not found." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetByUser");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetByUser");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Récupère une wishlist avec ses items associés.
    [HttpGet("with-items/{wishlistId:guid}")]
    public async Task<IActionResult> GetWithItems(Guid wishlistId)
    {
        try
        {
            if (wishlistId == Guid.Empty) return BadRequest(new { Error = "Invalid wishlistId" });
            Wishlist? wishlist = await _wishlistService.GetWithItemsAsync(wishlistId);
            return wishlist != null ? Ok(wishlist) : NotFound(new { Error = "Wishlist not found." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetWithItems");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetWithItems");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
}
