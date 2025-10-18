using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service de l'historique de recherche.
/// </summary>
public class SearchHistoryService : Service<SearchHistory>, ISearchHistoryService
{
    private readonly ISearchHistoryRepository _searchHistoryRepository;

    public SearchHistoryService(ISearchHistoryRepository searchHistoryRepository, ILogger<SearchHistoryService> logger)
        : base(searchHistoryRepository, logger)
    {
        _searchHistoryRepository = searchHistoryRepository;
    }

    public Task<IEnumerable<SearchHistory>> GetByUserAsync(Guid userId, int take = 50)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Invalid userId", nameof(userId));
        return _searchHistoryRepository.GetByUserAsync(userId, take);
    }

    public Task<IEnumerable<(string Term, int Count)>> GetTopTermsAsync(int top = 10)
    {
        return _searchHistoryRepository.GetTopTermsAsync(top);
    }

    public Task AddTermAsync(Guid userId, string term)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Invalid userId", nameof(userId));
        if (string.IsNullOrWhiteSpace(term)) throw new ArgumentException("Term requis", nameof(term));
        return _searchHistoryRepository.AddTermAsync(userId, term);
    }
}