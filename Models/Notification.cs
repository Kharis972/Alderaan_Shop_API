namespace alderaan_shop.Models;

// Entité représentant une notification envoyée à un utilisateur.

public class Notification
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Type { get; init; } 
    public string Title { get; init; }
    public string Message { get; init; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ReadAt { get; set; }
    
    // Navigation
    public User User { get; set; }
    
    private Notification() {}
    
    // Crée une notification utilisateur.
    public Notification(
        Guid id,
        Guid userId,
        Memory<char> type,
        Memory<char> title,
        Memory<char> message
    )
    {
        Id = id;
        UserId = userId;
        Type = type.ToString();
        Title = title.ToString();
        Message = message.ToString();
        IsRead = false;
        CreatedAt = DateTime.UtcNow;
    }
}