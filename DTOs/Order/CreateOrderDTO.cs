using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alderaan_shop.DTOs.Order;

public class CreateOrderDTO
{
    [Required]
    [EmailAddress]
    [MinLength(6)]
    [MaxLength(100)]
    public required string Mail { get; init; }

    [Required]
    [MinLength(10)]
    [MaxLength(256)]
    public required string ShippingAddress { get; init; }

    [Required]
    [RegularExpression(@"^\d{5}$")]
    public required string ShippingZipCode { get; init; }

    [Required]
    [MinLength(10)]
    [MaxLength(256)]
    public required string BillingAddress { get; init; }

    [Required]
    [RegularExpression(@"^\d{5}$")]
    public required string BillingZipCode { get; init; }

    [Required]
    [MinLength(1)]
    public required List<CreateOrderItemDTO> Items { get; init; }

    [NotMapped]
    public Memory<char> MailMemoryChar;
    [NotMapped]
    public Memory<char> ShippingAddressMemoryChar;
    [NotMapped]
    public Memory<char> ShippingZipCodeMemoryChar;
    [NotMapped]
    public Memory<char> BillingAddressMemoryChar;
    [NotMapped]
    public Memory<char> BillingZipCodeMemoryChar;

    public void InitializeMemoryChar()
    {
        MailMemoryChar = Mail.ToCharArray().AsMemory();
        ShippingAddressMemoryChar = ShippingAddress.ToCharArray().AsMemory();
        ShippingZipCodeMemoryChar = ShippingZipCode.ToCharArray().AsMemory();
        BillingAddressMemoryChar = BillingAddress.ToCharArray().AsMemory();
        BillingZipCodeMemoryChar = BillingZipCode.ToCharArray().AsMemory();
    }
}

public class CreateOrderItemDTO
{
    [Required]
    public Guid ProductId { get; init; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; init; }
}