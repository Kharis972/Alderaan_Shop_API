namespace alderaan_shop.Models;

// Entrée d’historique de recherche utilisateur.
public class SearchHistory
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string SearchQuery { get; init; }
    public int ResultCount { get; init; }
    public DateTime SearchedAt { get; init; }
    
    // Navigation
    public User User { get; set; }
    
    private SearchHistory() {}
    
    // Crée une entrée dans l'historique des recherches.
    public SearchHistory(
        Guid id,
        Guid userId,
        Memory<char> searchQuery,
        int resultCount
    )
    {
        Id = id;
        UserId = userId;
        SearchQuery = searchQuery.ToString();
        ResultCount = resultCount;
        SearchedAt = DateTime.UtcNow;
    }
}