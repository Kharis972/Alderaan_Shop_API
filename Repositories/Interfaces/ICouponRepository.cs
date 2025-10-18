using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité Coupon.
/// </summary>
public interface ICouponRepository : IRepository<Coupon>
{
    Task<Coupon?> FindByCodeAsync(string code);
    Task<IEnumerable<Coupon>> GetActiveAsync();
    Task<IEnumerable<Coupon>> GetValidNowAsync(DateTime nowUtc);
}
