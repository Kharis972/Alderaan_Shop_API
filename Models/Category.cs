namespace alderaan_shop.Models;


// Entité représentant une catégorie de produits, potentiellement hiérarchique.

public class Category
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public Guid? ParentCategoryId { get; init; } 
    public string ImageUrl { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    
    // Navigation
    public ICollection<Product> Products { get; set; }
    public Category? ParentCategory { get; set; }
    
    private Category() {}
    
    // Crée une catégorie de produits, optionnellement rattachée à une catégorie parente.
    public Category(
        Guid id,
        Memory<char> name,
        Memory<char> description,
        Guid? parentCategoryId,
        Memory<char> imageUrl
    )
    {
        Id = id;
        Name = name.ToString();
        Description = description.ToString();
        ParentCategoryId = parentCategoryId;
        ImageUrl = imageUrl.ToString();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
}