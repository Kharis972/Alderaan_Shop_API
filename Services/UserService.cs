using alderaan_shop.DTOs.User;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Helpers.Encryption;
using alderaan_shop.Models;
using alderaan_shop.Models.ClientSided;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

public class UserService : Service<User>, IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository, ILogger<UserService> logger) : base(userRepository, logger)
    {
        _userRepository = userRepository;
        Logger = logger;
    }

    public async Task SignupAsync(NewUserDTO newUserDto)
    {
        try
        {
            //Boolean that checks whether a user exists based on their email and phone number.
            if (await _userRepository.CheckForUserByUniqueIndexesAsync(
                    Encryption.ComputeUniqueHmac(newUserDto.MailMemoryChar).ToString(),
                    Encryption.ComputeUniqueHmac(newUserDto.PhoneNumberMemoryChar).ToString()))
                throw new UserAlreadyExistsException("A user with this information is already registered.");
            Guid id = Guid.NewGuid();
            //As long as the ID exists in the database, it will keep generating a new one; otherwise, it stops.
            while (await _userRepository.CheckForUserByIdAsync(id))
            {
                //The system will regenerate the ID to ensure uniqueness.
                id = Guid.NewGuid();
            }
            
            // Génère un identifiant unique pour le token/session
            Memory<char> jti = Guid.NewGuid().ToString().ToCharArray().AsMemory();
            DateTime sessionExpiresAt = DateTime.UtcNow.AddHours(24);
            
            //Instantiates newUser using a constructor that accepts cipher parameters for encryption or HMAC with a hash and key.
            User user = new User(
                id,
                Encryption.CipherData(newUserDto.FirstNameMemoryChar),
                Encryption.CipherData(newUserDto.LastNameMemoryChar),
                Encryption.CipherData(newUserDto.AddressMemoryChar),
                Encryption.CipherData(newUserDto.ZipCodeMemoryChar),
                Encryption.CipherData(newUserDto.MailMemoryChar),
                Encryption.CipherData(newUserDto.PhoneNumberMemoryChar),
                await Encryption.HashPassword(newUserDto.PasswordMemoryChar),
                Encryption.ComputeUniqueHmac(newUserDto.MailMemoryChar),
                Encryption.ComputeUniqueHmac(newUserDto.PhoneNumberMemoryChar),
                Encryption.ComputeUniqueHmac(newUserDto.SecurityQuestionMemoryChar),
                Encryption.ComputeUniqueHmac(newUserDto.SecurityAnswerMemoryChar),
                Guid.NewGuid().ToString().ToCharArray().AsMemory(),
                sessionExpiresAt
            );

            await _userRepository.SaveToDatabaseAsync(user);
        }
        catch (Exception e) when (e is UserAlreadyExistsException or DatabaseException)
        {
            Console.WriteLine("Exception relancée : " + e.Message);
            throw;
        }
        catch (Exception e)
        {
            Logger.LogError(e,"The user could not be created.");
            throw new InternalServerException("Your attempt has failed.");
        }
    }

    public async Task<(string, ClientSidedUser)> LoginAsync(LoginDTO loginDto)
    {
        try
        {
            User? userToLogin = await _userRepository.GetUserByMailHmacAsync(
                Encryption.ComputeUniqueHmac(loginDto.MailMemoryChar).ToString()); 
            if (userToLogin == null)
                throw new WrongCredentialsException("Incorrect credentials. Please check your username and password.");
            if (!await Encryption.VerifyPassword(loginDto.PasswordMemoryChar, userToLogin.EncryptedPassword))
                throw new WrongCredentialsException("Incorrect credentials. Please check your username and password.");
            
            //Returns a (Guid, ClientSidedUser) tuple.
            //How to use this tuple in the UserController: (string userId, ClientSidedUser user) = await _userService.LoginAsync(loginDto);

            return (
                userToLogin.Id.ToString(), new ClientSidedUser(
                    Encryption.DecipherData(userToLogin.FirstName),
                    Encryption.DecipherData(userToLogin.LastName),
                    Encryption.DecipherData(userToLogin.Address),
                    Encryption.DecipherData(userToLogin.ZipCode),
                    loginDto.Mail,
                    Encryption.DecipherData(userToLogin.PhoneNumber)));
        }
        catch (Exception e) when (e is WrongCredentialsException or IncorretLoginInfosException)
        {
            throw;
        }
        catch (Exception e)
        {
            Logger.LogCritical(e, "Login attempt failed.");
            throw new InternalServerException("You cannot log in.");
        }
    }
}