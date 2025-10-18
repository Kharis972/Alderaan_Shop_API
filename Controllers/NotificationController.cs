using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour les notifications utilisateur.
/// Route de base: api/Notification
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }
    
    /// Retourne les notifications d'un utilisateur (récents d'abord).
    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, [FromQuery] int take = 50)
    {
        try
        {
            if (userId == Guid.Empty) return BadRequest(new { Error = "Invalid userId" });
            IEnumerable<Notification> list = await _notificationService.GetByUserAsync(userId, take);
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

    /// <summary>
    /// Retourne les notifications non lues d'un utilisateur.
    /// </summary>
    [HttpGet("unread/{userId:guid}")]
    public async Task<IActionResult> GetUnread(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty) return BadRequest(new { Error = "Invalid userId" });
            IEnumerable<Notification> list = await _notificationService.GetUnreadByUserAsync(userId);
            return Ok(list);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in GetUnread");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetUnread");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }

    /// <summary>
    /// Marque une notification comme lue.
    /// </summary>
    [HttpPost("mark-read/{notificationId:guid}")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId)
    {
        try
        {
            if (notificationId == Guid.Empty) return BadRequest(new { Error = "Invalid notificationId" });
            bool ok = await _notificationService.MarkAsReadAsync(notificationId);
            return ok ? Ok(new { Message = "Notification marked as read." }) : NotFound(new { Error = "Notification not found." });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument in MarkAsRead");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in MarkAsRead");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
}
