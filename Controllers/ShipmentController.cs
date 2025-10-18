using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour les expéditions.
/// Route de base: api/Shipment
[ApiController]
[Route("api/[controller]")]
public class ShipmentController : ControllerBase
{
    private readonly IShipmentService _shipmentService;
    private readonly ILogger<ShipmentController> _logger;

    public ShipmentController(IShipmentService shipmentService, ILogger<ShipmentController> logger)
    {
        _shipmentService = shipmentService;
        _logger = logger;
    }
    
    /// Récupère une expédition par identifiant de commande.
    [HttpGet("by-order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrderId(Guid orderId)
    {
        try
        {
            if (orderId == Guid.Empty) return BadRequest(new { Error = "Invalid orderId" });
            Shipment? shipment = await _shipmentService.GetByOrderIdAsync(orderId);
            return shipment != null ? Ok(shipment) : NotFound(new { Error = "Shipment not found." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetByOrderId");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetByOrderId");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Récupère une expédition par numéro de suivi.
    [HttpGet("by-tracking/{trackingId}")]
    public async Task<IActionResult> GetByTrackingId(string trackingId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(trackingId)) return BadRequest(new { Error = "TrackingId requis" });
            Shipment? shipment = await _shipmentService.GetByTrackingIdAsync(trackingId);
            return shipment != null ? Ok(shipment) : NotFound(new { Error = "Shipment not found." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetByTrackingId");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetByTrackingId");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Met à jour le statut d'une expédition.
    [HttpPost("{shipmentId:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid shipmentId, [FromQuery] string status)
    {
        try
        {
            if (shipmentId == Guid.Empty) return BadRequest(new { Error = "Invalid shipmentId" });
            if (string.IsNullOrWhiteSpace(status)) return BadRequest(new { Error = "Status requis" });
            bool ok = await _shipmentService.UpdateStatusAsync(shipmentId, status);
            return ok ? Ok(new { Message = "Shipment status updated." }) : NotFound(new { Error = "Shipment not found." });
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
