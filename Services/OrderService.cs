using alderaan_shop.Data;
using alderaan_shop.DTOs.Order;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Helpers.Encryption;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Services;

/// <summary>
/// Service de gestion des commandes.
/// Responsable de la validation, de l'orchestration des écritures (commande, items, paiement),
/// et de la mise à jour du stock au sein d'une transaction.
/// </summary>
public class OrderService : IOrderService
{
    private readonly ApplicationDatabaseContext _db;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<OrderService> _logger;

    /// <summary>
    /// Constructeur avec injection du DbContext, du repository utilisateur et du logger.
    /// </summary>
    public OrderService(ApplicationDatabaseContext db, IUserRepository userRepository, ILogger<OrderService> logger)
    {
        _db = db;
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <summary>
    /// Crée une commande complète (Order, OrderItems, Payment) à partir du DTO fourni.
    /// - Vérifie l'utilisateur par HMAC de l'email
    /// - Valide les produits, quantités et stock
    /// - Persiste le tout au sein d'une transaction et met à jour le stock
    /// </summary>
    public async Task<(Guid OrderId, string PaymentStatus)> PlaceOrderAsync(CreateOrderDTO dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            throw new IncorretLoginInfosException("No order items provided.");

        dto.InitializeMemoryChar();

        // Recherche de l'utilisateur via HMAC (email)
        string emailHmac = Encryption.ComputeUniqueHmac(dto.MailMemoryChar).ToString();
        User? user = await _userRepository.GetUserByMailHmacAsync(emailHmac);
        if (user == null)
            throw new WrongCredentialsException("User not found.");

        // Charger les produits et valider
        Dictionary<Guid, Models.Product> products = new();
        foreach (CreateOrderItemDTO item in dto.Items)
        {
            Models.Product? product = await _db.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
            if (product == null)
                throw new ProductNotFoundException($"Product {item.ProductId} not found.");
            if (item.Quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(item.Quantity), "Quantity must be at least 1.");
            if (product.Stock < item.Quantity)
                throw new InvalidOperationException($"Insufficient stock for product {product.Id}.");
            products[item.ProductId] = product;
        }

        Guid orderId = Guid.NewGuid();
        decimal total = 0m;
        List<OrderItem> orderItems = new();

        foreach (CreateOrderItemDTO item in dto.Items)
        {
            Models.Product product = products[item.ProductId];
            decimal lineTotal = product.Price * item.Quantity;
            total += lineTotal;

            orderItems.Add(new OrderItem
            (
                id: Guid.NewGuid(),
                orderId: orderId,
                productId: product.Id,
                quantity: item.Quantity,
                unitPrice: product.Price
            ));
        }

        // Créer la commande
        Order order = new Order(
            id: orderId,
            userId: user.Id,
            totalAmount: total,
            shippingAddress: dto.ShippingAddressMemoryChar,
            shippingZipCode: dto.ShippingZipCodeMemoryChar,
            billingAddress: dto.BillingAddressMemoryChar,
            billingZipCode: dto.BillingZipCodeMemoryChar
        );

        // Créer un paiement factice (en attente)
        Payment payment = new Payment(
            id: Guid.NewGuid(),
            orderId: orderId,
            amount: total,
            paymentMethod: "Mock".ToCharArray().AsMemory(),
            transactionId: ($"TX-{Guid.NewGuid():N}").ToCharArray().AsMemory()
        );

        // Persister au sein d'une transaction
        using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            await _db.Orders.AddAsync(order);
            await _db.OrderItems.AddRangeAsync(orderItems);
            await _db.Payments.AddAsync(payment);

            // Décrémenter le stock
            foreach (CreateOrderItemDTO item in dto.Items)
            {
                Models.Product product = products[item.ProductId];
                product.Stock -= item.Quantity;
                product.SoldCount += item.Quantity;
                _db.Products.Update(product);
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }
        catch (DbUpdateException ex)
        {
            await tx.RollbackAsync();
            string detailedMessage = ex.InnerException?.Message ?? ex.Message;
            _logger.LogError(ex, "Database error while placing order. Details: {Details}", detailedMessage);
            throw new DatabaseException($"Database error: {detailedMessage}");
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            _logger.LogError(ex, "Unexpected error while placing order.");
            throw;
        }

        return (orderId, payment.Status);
    }
}