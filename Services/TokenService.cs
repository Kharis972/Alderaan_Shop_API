using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using alderaan_shop.Helpers.Encryption;
using alderaan_shop.Models.ClientSided;
using alderaan_shop.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace alderaan_shop.Services;

/// <summary>
/// Implémentation du service de génération de jetons JWT signés (JWS) et chiffrés (JWE).
/// - JWS: JSON Web Signature, un JWT signé pour garantir l'intégrité et l'authenticité.
/// - JWE: JSON Web Encryption, un JWT à la fois signé et chifré pour la confidentialité du contenu.
/// </summary>
public class TokenService : ITokenService
{
    private readonly JwtOptions _options;
    private readonly SigningCredentials _signingCredentials;
    private readonly EncryptingCredentials _encryptingCredentials;

    /// <summary>
    /// Initialise le service de jetons avec les options JWT et prépare les informations
    /// de signature (JWS) et de chiffrement (JWE).
    /// </summary>
    /// <param name="options">Options JWT (Issuer, Audience, durées, clés, etc.).</param>
    /// <exception cref="ArgumentException">
    /// Levée si la clé de signature (<c>SigningKey</c>) ou de chiffrement (<c>EncryptionKey</c>)
    /// est absente ou trop courte (minimum 32 caractères pour assurer une force 256 bits).
    /// </exception>
    public TokenService(IOptions<JwtOptions> options)
    {
        // Récupération des options typées (appsettings.json / configuration)
        _options = options.Value;

        // Contrôle de robustesse: on exige au moins 32 caractères pour des clés 256 bits.
        if (string.IsNullOrWhiteSpace(_options.SigningKey) || _options.SigningKey.Length < 32)
        {
            throw new ArgumentException("Jwt.SigningKey doit faire au moins 32 caractères pour HMAC-SHA256.");
        }
        if (string.IsNullOrWhiteSpace(_options.EncryptionKey) || _options.EncryptionKey.Length < 32)
        {
            throw new ArgumentException("Jwt.EncryptionKey doit faire au moins 32 caractères (256 bits).");
        }

        // Prépare la clé symétrique pour signer les JWS (HMAC-SHA256)
        SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        _signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // Prépare la clé symétrique pour chiffrer les JWE
        SymmetricSecurityKey encKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.EncryptionKey));
        // Utiliser A256KW pour la gestion de clé + A256CBC-HS512 pour le chiffrement du contenu (fort et standardisé)
        _encryptingCredentials = new EncryptingCredentials(
            encKey,
            SecurityAlgorithms.Aes256KW,
            SecurityAlgorithms.Aes256CbcHmacSha512);
    }

    /// <summary>
    /// Construit l'ensemble des revendications (claims) standards du JWT pour l'utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant unique de l'utilisateur (Sub).</param>
    /// <param name="user">Informations côté client sur l'utilisateur (email, prénom, nom).</param>
    /// <returns>La liste des claims à intégrer dans le token.</returns>
    public IEnumerable<Claim> BuildClaims(Guid userId, ClientSidedUser user)
    {
        // Claims de base: subject, identifiant unique du token, émetteur et audience
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // Sujet du JWT (utilisateur)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID unique du token
            new Claim(JwtRegisteredClaimNames.Iss, _options.Issuer), // Émetteur déclaré
            new Claim(JwtRegisteredClaimNames.Aud, _options.Audience), // Audience visée
        };

        // Claims optionnels selon les infos disponibles
        if (!string.IsNullOrWhiteSpace(user?.Mail))
        {
            claims.Add(new Claim(ClaimTypes.Email, user.Mail));
        }
        if (!string.IsNullOrWhiteSpace(user?.FirstName))
        {
            claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
        }
        if (!string.IsNullOrWhiteSpace(user?.LastName))
        {
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
        }

        return claims;
    }

    /// <summary>
    /// Génère un JWT signé (JWS) non chiffré contenant les claims de l'utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant unique de l'utilisateur.</param>
    /// <param name="user">Données utilisateur pour enrichir les claims.</param>
    /// <returns>Le jeton JWS signé (chaîne encodée en compact).</returns>
    public string GenerateJws(Guid userId, ClientSidedUser user)
    {
        // Définition de la fenêtre de validité du token
        DateTime now = DateTime.UtcNow;
        DateTime expires = now.AddMinutes(_options.AccessTokenMinutes);

        // Récupère les claims applicables à l'utilisateur
        IEnumerable<Claim> claims = BuildClaims(userId, user);

        // Construit le JWT et le signe via HMAC-SHA256
        JwtSecurityToken jwt = new(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: _signingCredentials);

        // Sérialise le token en format compact (header.payload.signature)
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <summary>
    /// Génère un JWT chiffré (JWE) en plus d'être signé, pour la confidentialité des claims.
    /// </summary>
    /// <param name="userId">Identifiant unique de l'utilisateur.</param>
    /// <param name="user">Données utilisateur pour enrichir les claims.</param>
    /// <returns>Le jeton JWE signé et chiffré (chaîne encodée en compact).</returns>
    public string GenerateJwe(Guid userId, ClientSidedUser user)
    {
        // Définition de la fenêtre de validité du token
        DateTime now = DateTime.UtcNow;
        DateTime expires = now.AddMinutes(_options.AccessTokenMinutes);

        // Récupère les claims applicables à l'utilisateur
        IEnumerable<Claim> claims = BuildClaims(userId, user);

        // Prépare la description du token: signature + chiffrement
        SecurityTokenDescriptor descriptor = new()
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Subject = new ClaimsIdentity(claims),
            NotBefore = now,
            Expires = expires,
            SigningCredentials = _signingCredentials, // garantit l'intégrité/authenticité
            EncryptingCredentials = _encryptingCredentials // garantit la confidentialité
        };

        // Création et sérialisation du JWE
        JwtSecurityTokenHandler handler = new();
        SecurityToken token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
}