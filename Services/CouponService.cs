using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service coupon: logique légère et délégation au repository.
/// </summary>
public class CouponService : Service<Coupon>, ICouponService
{
    private readonly ICouponRepository _couponRepository;

    /// <summary>
    /// Initialise le service des coupons.
    /// </summary>
    /// <param name="couponRepository">Repository des coupons.</param>
    /// <param name="logger">Logger typé.</param>
    public CouponService(ICouponRepository couponRepository, ILogger<CouponService> logger)
        : base(couponRepository, logger)
    {
        _couponRepository = couponRepository;
    }

    /// <summary>
    /// Recherche un coupon par code exact.
    /// </summary>
    /// <param name="code">Code promotionnel.</param>
    /// <returns>Le coupon trouvé ou null.</returns>
    /// <exception cref="ArgumentException">Si code est vide.</exception>
    public Task<Coupon?> FindByCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code requis", nameof(code));
        return _couponRepository.FindByCodeAsync(code);
    }

    /// <summary>
    /// Liste les coupons actifs.
    /// </summary>
    public Task<IEnumerable<Coupon>> GetActiveAsync()
    {
        return _couponRepository.GetActiveAsync();
    }

    /// <summary>
    /// Liste les coupons valides à l'instant donné.
    /// </summary>
    /// <param name="nowUtc">Horodatage UTC.</param>
    public Task<IEnumerable<Coupon>> GetValidNowAsync(DateTime nowUtc)
    {
        return _couponRepository.GetValidNowAsync(nowUtc);
    }
}