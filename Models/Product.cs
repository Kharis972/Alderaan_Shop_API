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
    
    private Product() {}
    
    public Product
    (
        string name, 
        string description, 
        decimal price, 
        string imageUrl, 
        int stock
    )
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
        Stock = stock;
        CreatedAt = DateTime.UtcNow;
    }
}