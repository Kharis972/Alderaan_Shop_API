namespace alderaan_shop.Helpers.Encryption;

/// <summary>
/// Options de configuration pour la génération et la validation des JWT/JWE.
/// </summary>
public class JwtOptions
{
    /// <summary>Émetteur (iss).</summary>
    public string Issuer { get; set; } = "alderaan_shop";

    /// <summary>Audience (aud).</summary>
    public string Audience { get; set; } = "alderaan_shop_clients";

    /// <summary>Clé secrète de signature HMAC (au moins 32 caractères).</summary>
    public string SigningKey { get; set; } = string.Empty;

    /// <summary>Clé secrète d'encapsulation/chiffrement JWE (32 octets recommandés).</summary>
    public string EncryptionKey { get; set; } = string.Empty;

    /// <summary>Durée de vie en minutes du jeton d'accès.</summary>
    public int AccessTokenMinutes { get; set; } = 60;
}