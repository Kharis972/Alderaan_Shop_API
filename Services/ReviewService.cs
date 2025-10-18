using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service des avis produits.
/// </summary>
public class ReviewService : Service<Review>, IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository, ILogger<ReviewService> logger)
        : base(reviewRepository, logger)
    {
        _reviewRepository = reviewRepository;
    }

    public Task<IEnumerable<Review>> GetByProductAsync(Guid productId)
    {
        if (productId == Guid.Empty) throw new ArgumentException("Invalid productId", nameof(productId));
        return _reviewRepository.GetByProductAsync(productId);
    }

    public Task<IEnumerable<Review>> GetByUserAsync(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Invalid userId", nameof(userId));
        return _reviewRepository.GetByUserAsync(userId);
    }

    public Task<double> GetAverageRatingAsync(Guid productId)
    {
        if (productId == Guid.Empty) throw new ArgumentException("Invalid productId", nameof(productId));
        return _reviewRepository.GetAverageRatingAsync(productId);
    }

    public Task<IEnumerable<Review>> GetRecentByProductAsync(Guid productId, int take = 10)
    {
        if (productId == Guid.Empty) throw new ArgumentException("Invalid productId", nameof(productId));
        return _reviewRepository.GetRecentByProductAsync(productId, take);
    }
}