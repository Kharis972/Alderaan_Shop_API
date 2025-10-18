using alderaan_shop.DTOs.User;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Helpers.Encryption;
using alderaan_shop.Models;
using alderaan_shop.Models.ClientSided;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

/// Contrôleur REST pour la gestion des utilisateurs (inscription, connexion).
/// Route de base: api/User
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    
    /// Constructeur avec injection du service utilisateur et du logger.
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;

    }
    
    /// Inscrit un nouvel utilisateur.
    /// <param name="newUserDto">Données d'inscription utilisateur.</param>
    /// <returns>200 OK si succès, 409 si conflit (existe déjà), 500 sinon.</returns>
    /// <response code="200">Compte créé.</response>
    /// <response code="409">Conflit: utilisateur déjà existant.</response>
    /// <response code="500">Erreur interne.</response>
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] NewUserDTO newUserDto)
    {
        try
        {
            newUserDto.InitializeMemoryChar();
            await _userService.SignupAsync(newUserDto);

            Console.WriteLine("SignupAsync called");
            return Ok(new
            {
                Message = "Account successfully created."
            });
        }
        catch (UserAlreadyExistsException ex)
        {
            _logger.LogWarning($"Signup failed for email: {newUserDto.Mail}. Reason: {ex.Message}");
            return Conflict("Unable to create account. Please try again.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Signup HMAC mail: " + Encryption.ComputeUniqueHmac(newUserDto.MailMemoryChar));
            Console.WriteLine("Erreur dans SignupAsync : " + ex.ToString());
            _logger.LogError(ex, "Unexpected error during signup.");
            return StatusCode(500, "An error occurred. Please try again later.");
        }
    }
    
    /// Authentifie un utilisateur via email et mot de passe.
    /// <param name="loginDto">Identifiants de connexion.</param>
    /// <returns>200 OK si succès (avec projection ClientSidedUser), 401 si identifiants invalides, 409/500 sinon.</returns>
    /// <response code="200">Connexion réussie.</response>
    /// <response code="401">Identifiants invalides.</response>
    /// <response code="409">Erreur côté serveur.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            // Initialisation de MailMemoryChar et PasswordMemoryChar
            loginDto.InitializeMemoryChar();
            (string userId, ClientSidedUser user) = await _userService.LoginAsync(loginDto);

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(new
            {
                Message = "User successfully logged in.",
                UserId = userId,
                User = user
            });
        }
        catch (WrongCredentialsException ex)
        {
            // Log côté serveur
            _logger.LogWarning($"Échec de connexion pour l'email : {loginDto.Mail}. Raison : {ex.Message}");

            _logger.LogInformation("HMAC calculé : " + Encryption.ComputeUniqueHmac(loginDto.MailMemoryChar));
            _logger.LogInformation("Longueur de PasswordMemoryChar : " + loginDto.PasswordMemoryChar.Length);

            // Log côté client
            return Unauthorized("Login failed. Please check your credentials.");
        }
        catch (Exception ex)
        {
            // Log complet
            _logger.LogError(ex, "Erreur inattendue lors de la connexion.");

            Console.WriteLine("HMAC du mail lors du login : " + Encryption.ComputeUniqueHmac(loginDto.MailMemoryChar));
            
            // Log côté client
            return Conflict("An error occurred. Please try again later.");
        }
    }
}