namespace alderaan_shop.Models;

// Entité d'adresse postale d'un utilisateur.
public class Address
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Label { get; init; } 
    public string FullAddress { get; init; }
    public string ZipCode { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; init; }
    
    // Navigation
    public User User { get; set; }
    
    private Address() {}
    
    // Crée une nouvelle adresse pour un utilisateur.
    // Les paramètres sensibles sont fournis en Memory<char> et convertis en string pour la persistance.
    public Address(
        Guid id,
        Guid userId,
        Memory<char> label,
        Memory<char> fullAddress,
        Memory<char> zipCode,
        Memory<char> city,
        Memory<char> country,
        bool isDefault
    )
    {
        Id = id;
        UserId = userId;
        Label = label.ToString();
        FullAddress = fullAddress.ToString();
        ZipCode = zipCode.ToString();
        City = city.ToString();
        Country = country.ToString();
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
    }
}