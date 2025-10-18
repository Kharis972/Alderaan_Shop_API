using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour les demandes de retour.
/// Route de base: api/ReturnRequest
[ApiController]
[Route("api/[controller]")]
public class ReturnRequestController : ControllerBase
{
    private readonly IReturnRequestService _returnRequestService;
    private readonly ILogger<ReturnRequestController> _logger;

    public ReturnRequestController(IReturnRequestService returnRequestService, ILogger<ReturnRequestController> logger)
    {
        _returnRequestService = returnRequestService;
        _logger = logger;
    }
    
    /// Retourne les demandes de retour d'un utilisateur.
    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty) return BadRequest(new { Error = "Invalid userId" });
            IEnumerable<ReturnRequest> list = await _returnRequestService.GetByUserAsync(userId);
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

    /// Retourne les demandes de retour d'une commande.
    [HttpGet("by-order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrder(Guid orderId)
    {
        try
        {
            if (orderId == Guid.Empty) return BadRequest(new { Error = "Invalid orderId" });
            IEnumerable<ReturnRequest> list = await _returnRequestService.GetByOrderAsync(orderId);
            return Ok(list);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetByOrder");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetByOrder");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }

    /// Met à jour le statut d'une demande de retour.
    [HttpPost("{returnRequestId:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid returnRequestId, [FromQuery] string status, [FromBody] string? adminNotes)
    {
        try
        {
            if (returnRequestId == Guid.Empty) return BadRequest(new { Error = "Invalid returnRequestId" });
            if (string.IsNullOrWhiteSpace(status)) return BadRequest(new { Error = "Status requis" });
            bool ok = await _returnRequestService.UpdateStatusAsync(returnRequestId, status, adminNotes);
            return ok ? Ok(new { Message = "Return request updated." }) : NotFound(new { Error = "Return request not found." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in UpdateStatus");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in UpdateStatus");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
}
