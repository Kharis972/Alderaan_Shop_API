using System.Security.Claims;
using alderaan_shop.Helpers.Encryption;
using alderaan_shop.Models.ClientSided;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service de génération de JWT (signé) et JWE (chiffré).
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Génère un JWT signé (JWS) pour l'utilisateur spécifié.
    /// </summary>
    /// <param name="userId">Identifiant utilisateur.</param>
    /// <param name="user">Projection côté client de l'utilisateur.</param>
    /// <returns>Chaîne du jeton JWT signé.</returns>
    string GenerateJws(Guid userId, ClientSidedUser user);

    /// <summary>
    /// Génère un JWE (JWT chiffré) pour l'utilisateur spécifié.
    /// </summary>
    /// <param name="userId">Identifiant utilisateur.</param>
    /// <param name="user">Projection côté client de l'utilisateur.</param>
    /// <returns>Chaîne du jeton JWE.</returns>
    string GenerateJwe(Guid userId, ClientSidedUser user);

    /// <summary>
    /// Construit une liste de revendications standard à partir de l'utilisateur.
    /// </summary>
    IEnumerable<Claim> BuildClaims(Guid userId, ClientSidedUser user);
}