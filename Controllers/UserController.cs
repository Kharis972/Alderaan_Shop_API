using alderaan_shop.DTOs.User;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Helpers.Encryption;
using alderaan_shop.Models;
using alderaan_shop.Models.ClientSided;
using alderaan_shop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alderaan_shop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;



    //Constructor to initialize the user service via dependency injection.
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;

    }

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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            //Init of MailMemoryChar and PasswordMemoryChar 
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
            // Log server side
            _logger.LogWarning($"Login failed for email: {loginDto.Mail}. Reason: {ex.Message}");

            _logger.LogInformation("Computed HMAC: " + Encryption.ComputeUniqueHmac(loginDto.MailMemoryChar));
            _logger.LogInformation("PasswordMemoryChar length: " + loginDto.PasswordMemoryChar.Length);

            // Log client side
            return Unauthorized("Login failed. Please check your credentials.");
        }
        catch (Exception ex)
        {
            // Log complete
            _logger.LogError(ex, "Unexpected error during login.");

            Console.WriteLine("Login HMAC mail: " + Encryption.ComputeUniqueHmac(loginDto.MailMemoryChar));
            
            // Log client side
            return Conflict("An error occurred. Please try again later.");
        }
    }
}