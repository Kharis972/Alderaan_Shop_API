using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour l'historique de recherche.
/// Route de base: api/SearchHistory
[ApiController]
[Route("api/[controller]")]
public class SearchHistoryController : ControllerBase
{
    private readonly ISearchHistoryService _searchHistoryService;
    private readonly ILogger<SearchHistoryController> _logger;

    public SearchHistoryController(ISearchHistoryService searchHistoryService, ILogger<SearchHistoryController> logger)
    {
        _searchHistoryService = searchHistoryService;
        _logger = logger;
    }
    
    /// Retourne les dernières recherches d'un utilisateur.
    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, [FromQuery] int take = 50)
    {
        try
        {
            if (userId == Guid.Empty) return BadRequest(new { Error = "Invalid userId" });
            IEnumerable<SearchHistory> list = await _searchHistoryService.GetByUserAsync(userId, take);
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
    
    /// Retourne les termes les plus recherchés.
    [HttpGet("top-terms")]
    public async Task<IActionResult> GetTopTerms([FromQuery] int top = 10)
    {
        try
        {
            IEnumerable<(string Term, int Count)> list = await _searchHistoryService.GetTopTermsAsync(top);
            return Ok(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetTopTerms");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Ajoute un terme dans l'historique d'un utilisateur.
    [HttpPost("add-term")]
    public async Task<IActionResult> AddTerm([FromQuery] Guid userId, [FromBody] string term)
    {
        try
        {
            if (userId == Guid.Empty) return BadRequest(new { Error = "Invalid userId" });
            if (string.IsNullOrWhiteSpace(term)) return BadRequest(new { Error = "Term requis" });
            await _searchHistoryService.AddTermAsync(userId, term);
            return Ok(new { Message = "Term added." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in AddTerm");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in AddTerm");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
}
