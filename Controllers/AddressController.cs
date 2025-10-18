using alderaan_shop.Models;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour la gestion des adresses utilisateur.
/// Route de base: api/Address
[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;
    private readonly ILogger<AddressController> _logger;
    
    /// Constructeur avec injection du service et du logger.
    public AddressController(IAddressService addressService, ILogger<AddressController> logger)
    {
        _addressService = addressService;
        _logger = logger;
    }
    
    /// Retourne toutes les adresses d'un utilisateur.
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <response code="200">Liste des adresses retournée.</response>
    /// <response code="400">Paramètre invalide.</response>
    /// <response code="500">Erreur interne.</response>
    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty) return BadRequest(new { Error = "Invalid userId" });
            IEnumerable<Address> addresses = await _addressService.GetByUserIdAsync(userId);
            return Ok(addresses);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument for GetByUser");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetByUser");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
    
    /// Retourne l'adresse par défaut d'un utilisateur, si définie.
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <response code="200">Adresse par défaut (ou null) retournée.</response>
    /// <response code="400">Paramètre invalide.</response>
    /// <response code="500">Erreur interne.</response>
    [HttpGet("default/{userId:guid}")]
    public async Task<IActionResult> GetDefault(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty) return BadRequest(new { Error = "Invalid userId" });
            Address? address = await _addressService.GetDefaultForUserAsync(userId);
            return Ok(address);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument for GetDefault");
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetDefault");
            return StatusCode(500, new { Error = "An unexpected error occurred." });
        }
    }
}
