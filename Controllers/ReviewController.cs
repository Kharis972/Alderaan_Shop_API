using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour les avis produits.
/// Route de base: api/Review
[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
    {
        _reviewService = reviewService;
        _logger = logger;
    }
    
    /// Récupère les avis pour un produit donné.
    [HttpGet("by-product/{productId:guid}")]
    public async Task<IActionResult> GetByProduct(Guid productId)
    {
        try
        {
            if (productId == Guid.Empty) return BadRequest(new { Error = "Invalid productId" });
            IEnumerable<Review> list = await _reviewService.GetByProductAsync(productId);
            return Ok(list);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetByProduct");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetByProduct");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Récupère les avis d'un utilisateur.
    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty) return BadRequest(new { Error = "Invalid userId" });
            IEnumerable<Review> list = await _reviewService.GetByUserAsync(userId);
            return Ok(list);
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
    
    /// Calcule la note moyenne d'un produit.
    [HttpGet("avg-rating/{productId:guid}")]
    public async Task<IActionResult> GetAverageRating(Guid productId)
    {
        try
        {
            if (productId == Guid.Empty) return BadRequest(new { Error = "Invalid productId" });
            double value = await _reviewService.GetAverageRatingAsync(productId);
            return Ok(new { Average = value });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetAverageRating");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetAverageRating");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Retourne les avis les plus récents pour un produit.
    [HttpGet("recent/{productId:guid}")]
    public async Task<IActionResult> GetRecent(Guid productId, [FromQuery] int take = 10)
    {
        try
        {
            if (productId == Guid.Empty) return BadRequest(new { Error = "Invalid productId" });
            IEnumerable<Review> list = await _reviewService.GetRecentByProductAsync(productId, take);
            return Ok(list);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetRecent");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetRecent");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
}
