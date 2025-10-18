using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour la newsletter.
/// Route de base: api/Newsletter
[ApiController]
[Route("api/[controller]")]
public class NewsletterController : ControllerBase
{
    private readonly INewsletterService _newsletterService;
    private readonly ILogger<NewsletterController> _logger;

    public NewsletterController(INewsletterService newsletterService, ILogger<NewsletterController> logger)
    {
        _newsletterService = newsletterService;
        _logger = logger;
    }
    
    /// Recherche un abonnement par email exact.
    [HttpGet("by-email/{email}")]
    public async Task<IActionResult> FindByEmail(string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest(new { Error = "Email requis" });
            Newsletter? entry = await _newsletterService.FindByEmailAsync(email);
            return Ok(entry);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in FindByEmail");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in FindByEmail");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Liste des abonnements actifs.
    [HttpGet("subscribed")]
    public async Task<IActionResult> GetSubscribed()
    {
        try
        {
            IEnumerable<Newsletter> list = await _newsletterService.GetSubscribedAsync();
            return Ok(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetSubscribed");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
}
