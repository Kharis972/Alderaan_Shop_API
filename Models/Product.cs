namespace alderaan_shop.Models;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; init; }
    
    // PROPRIÉTÉS :
    public Guid? CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public bool IsActive { get; set; }
    public int SoldCount { get; set; } // Pour "bestsellers"
    public decimal? DiscountedPrice { get; set; } // Prix après réduction
    
    // Navigation 
    public Category? Category { get; set; }
    public Brand? Brand { get; set; }
    public ICollection<ProductImage> Images { get; set; }
    public ICollection<Review> Reviews { get; set; }
    
    private Product() {}
    
    public Product(
        Guid id,
        Memory<char> name,
        Memory<char> description,
        decimal price,
        Memory<char> imageUrl,
        int stock,
        Guid? categoryId = null,
        Guid? brandId = null
    )
    {
        Id = id;
        Name = name.ToString();
        Description = description.ToString();
        Price = price;
        ImageUrl = imageUrl.ToString();
        Stock = stock;
        CategoryId = categoryId;
        BrandId = brandId;
        IsActive = true;
        SoldCount = 0;
        CreatedAt = DateTime.UtcNow;
    }
}