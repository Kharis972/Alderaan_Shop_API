using alderaan_shop.DTOs.Order;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST des commandes.
/// Route de base: api/Order
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;
    
    /// Constructeur avec injection du service de commande et du logger.
    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }
    
    /// Crée une nouvelle commande pour un utilisateur et décrémente les stocks.
    /// <param name="dto">Commande à créer (utilise l'HMAC de l'email pour retrouver l'utilisateur).</param>
    /// <returns>200 OK avec l'OrderId et l'état du paiement, codes d'erreur sinon.</returns>
    /// <response code="200">Commande créée.</response>
    /// <response code="404">Utilisateur ou produit introuvable.</response>
    /// <response code="400">Requête invalide.</response>
    /// <response code="500">Erreur interne / base de données.</response>
    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            (Guid orderId, string paymentStatus) = await _orderService.PlaceOrderAsync(dto);
            return Ok(new { Message = "Order created.", OrderId = orderId, PaymentStatus = paymentStatus });
        }
        catch (WrongCredentialsException ex)
        {
            _logger.LogWarning(ex, "Order creation failed: user not found");
            return NotFound(new { Error = "User not found." });
        }
        catch (ProductNotFoundException ex)
        {
            _logger.LogWarning(ex, "Order creation failed: product not found");
            return NotFound(new { Error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Order creation failed: invalid operation");
            return BadRequest(new { Error = ex.Message });
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Database error during order placement");
            // Afficher les erreurs DB détaillées en dev.

            return StatusCode(500, new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during order placement");
            return StatusCode(500, new { Error = "An unexpected error occurred while placing the order." });
        }
    }
}