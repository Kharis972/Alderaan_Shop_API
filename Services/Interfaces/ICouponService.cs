using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour les coupons de réduction.
/// </summary>
public interface ICouponService : IService<Coupon>
{
    /// <summary>
    /// Recherche un coupon par son code exact.
    /// </summary>
    /// <param name="code">Code promotionnel.</param>
    /// <returns>Le coupon s'il existe, sinon null.</returns>
    Task<Coupon?> FindByCodeAsync(string code);

    /// <summary>
    /// Retourne les coupons actifs.
    /// </summary>
    /// <returns>Une séquence de coupons actifs.</returns>
    Task<IEnumerable<Coupon>> GetActiveAsync();

    /// <summary>
    /// Retourne les coupons valides à l'instant donné (fenêtre [ValidFrom, ValidUntil]).
    /// </summary>
    /// <param name="nowUtc">Horodatage UTC de référence.</param>
    /// <returns>Une séquence de coupons valides.</returns>
    Task<IEnumerable<Coupon>> GetValidNowAsync(DateTime nowUtc);
}