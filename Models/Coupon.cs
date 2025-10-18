namespace alderaan_shop.Models;

// Entité représentant un coupon de réduction.
public class Coupon
{
    public Guid Id { get; init; }
    public string Code { get; init; }
    public string DiscountType { get; init; }
    public decimal DiscountValue { get; init; }
    public decimal? MinimumPurchase { get; init; }
    public int? UsageLimit { get; init; }
    public int UsageCount { get; set; }
    public DateTime ValidFrom { get; init; }
    public DateTime ValidUntil { get; init; }
    public bool IsActive { get; set; }
    
    private Coupon() {}
    
    // Crée un coupon de réduction configuré pour une période donnée.
    public Coupon(
        Guid id,
        Memory<char> code,
        Memory<char> discountType,
        decimal discountValue,
        decimal? minimumPurchase,
        int? usageLimit,
        DateTime validFrom,
        DateTime validUntil
    )
    {
        Id = id;
        Code = code.ToString();
        DiscountType = discountType.ToString();
        DiscountValue = discountValue;
        MinimumPurchase = minimumPurchase;
        UsageLimit = usageLimit;
        UsageCount = 0;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
        IsActive = true;
    }
}