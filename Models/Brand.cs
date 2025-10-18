namespace alderaan_shop.Models;

// Entité représentant une marque de produits.
public class Brand
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string LogoUrl { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    
    // Navigation
    public ICollection<Product> Products { get; set; }
    
    private Brand() {}
    
    // Crée une marque avec ses métadonnées principales.
    public Brand(
        Guid id,
        Memory<char> name,
        Memory<char> description,
        Memory<char> logoUrl
    )
    {
        Id = id;
        Name = name.ToString();
        Description = description.ToString();
        LogoUrl = logoUrl.ToString();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
}