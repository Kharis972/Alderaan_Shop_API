using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité Review.
/// </summary>
public interface IReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetByProductAsync(Guid productId);
    Task<IEnumerable<Review>> GetByUserAsync(Guid userId);
    Task<double> GetAverageRatingAsync(Guid productId);
    Task<IEnumerable<Review>> GetRecentByProductAsync(Guid productId, int take = 10);
}
