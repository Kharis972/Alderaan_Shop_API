using alderaan_shop.DTOs.User;
using alderaan_shop.Models;
using alderaan_shop.Models.ClientSided;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Contrat du service utilisateur.
/// Gère l'inscription et l'authentification, y compris la préparation de données sensibles (Memory<char>, HMAC, etc.).
/// </summary>
public interface IUserService : IService<User>
{
    /// <summary>
    /// Inscrit un nouvel utilisateur en validant et en chiffrant les données sensibles.
    /// </summary>
    Task SignupAsync(NewUserDTO newUserDto);

    /// <summary>
    /// Authentifie l'utilisateur et retourne son identifiant et une projection sécurisée pour le client.
    /// </summary>
    Task<(string, ClientSidedUser)> LoginAsync(LoginDTO loginDto);
}