using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour la gestion des coupons.
/// Route de base: api/Coupon
[ApiController]
[Route("api/[controller]")]
public class CouponController : ControllerBase
{
    private readonly ICouponService _couponService;
    private readonly ILogger<CouponController> _logger;

    public CouponController(ICouponService couponService, ILogger<CouponController> logger)
    {
        _couponService = couponService;
        _logger = logger;
    }
    
    /// Recherche un coupon par son code exact.
    /// <param name="code">Code promotionnel.</param>
    /// <response code="200">Coupon trouvé (ou null).</response>
    /// <response code="400">Paramètre invalide.</response>
    /// <response code="500">Erreur interne.</response>
    [HttpGet("by-code/{code}")]
    public async Task<IActionResult> FindByCode(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code)) return BadRequest(new { Error = "Code requis" });
            Coupon? coupon = await _couponService.FindByCodeAsync(code);
            return Ok(coupon);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in FindByCode");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in FindByCode");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Retourne les coupons actifs.
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            IEnumerable<Coupon> coupons = await _couponService.GetActiveAsync();
            return Ok(coupons);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetActive");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Retourne les coupons valides maintenant (UTC).
    [HttpGet("valid-now")]
    public async Task<IActionResult> GetValidNow()
    {
        try
        {
            IEnumerable<Coupon> coupons = await _couponService.GetValidNowAsync(DateTime.UtcNow);
            return Ok(coupons);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetValidNow");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
}
