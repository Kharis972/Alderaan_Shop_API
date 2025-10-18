namespace alderaan_shop.Models;

// Entité d'abonnement à la newsletter.
public class Newsletter
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public bool IsSubscribed { get; set; }
    public DateTime SubscribedAt { get; init; }
    public DateTime? UnsubscribedAt { get; set; }
    
    private Newsletter() {}
    
    // Crée un abonnement à la newsletter.
    public Newsletter(
        Guid id,
        Memory<char> email
    )
    {
        Id = id;
        Email = email.ToString();
        IsSubscribed = true;
        SubscribedAt = DateTime.UtcNow;
    }
}