using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité SearchHistory.
/// </summary>
public interface ISearchHistoryRepository : IRepository<SearchHistory>
{
    Task<IEnumerable<SearchHistory>> GetByUserAsync(Guid userId, int take = 50);
    Task<IEnumerable<(string Term, int Count)>> GetTopTermsAsync(int top = 10);
    Task AddTermAsync(Guid userId, string term);
}
