using alderaan_shop.Models;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Data;

public class ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : DbContext(options)
{
    // DbSets
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<ReturnRequest> ReturnRequests { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<WishlistItem> WishlistItems { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Newsletter> Newsletters { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ==================== PRODUCT ====================
        modelBuilder.Entity<Product>(product =>
        {
            product.ToTable("products");
            product.HasKey(p => p.Id);

            product.Property(p => p.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            product.Property(p => p.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            product.Property(p => p.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            product.Property(p => p.Price)
                .IsRequired()
                .HasColumnName("price")
                .HasColumnType("decimal(10,2)");

            product.Property(p => p.ImageUrl)
                .IsRequired()
                .HasColumnName("imageUrl")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            product.Property(p => p.Stock)
                .IsRequired()
                .HasColumnName("stock")
                .HasColumnType("int");

            product.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            product.Property(p => p.CategoryId)
                .HasColumnName("categoryId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            product.Property(p => p.BrandId)
                .HasColumnName("brandId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            // Relations
            // 1 Category -> N Products (optionnel)
            product.HasOne<Category>()
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // 1 Brand -> N Products (optionnel)
            product.HasOne<Brand>()
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ==================== USER ====================
        modelBuilder.Entity<User>(user =>
        {
            user.ToTable("users");
            user.HasKey(u => u.Id);

            user.Property(u => u.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            user.Property(u => u.FirstName)
                .IsRequired()
                .HasColumnName("firstName")
                .HasColumnType("varchar(512)")
                .HasMaxLength(512);

            user.Property(u => u.LastName)
                .IsRequired()
                .HasColumnName("lastName")
                .HasColumnType("varchar(512)")
                .HasMaxLength(512);

            user.Property(u => u.Address)
                .IsRequired()
                .HasColumnName("address")
                .HasColumnType("varchar(512)")
                .HasMaxLength(512);
            
            user.Property(u => u.ZipCode)
                .IsRequired()
                .HasColumnName("zipCode")
                .HasColumnType("varchar(512)")
                .HasMaxLength(512);
            
            user.Property(u => u.Mail)
                .IsRequired()
                .HasColumnName("mail")
                .HasColumnType("varchar(512)")
                .HasMaxLength(512);
            
            user.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasColumnName("phoneNumber")
                .HasColumnType("varchar(512)")
                .HasMaxLength(512);
            
            user.Property(u => u.EncryptedPassword)
                .IsRequired()
                .HasColumnName("encryptedPassword")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            user.Property(u => u.IsAdmin)
                .IsRequired()
                .HasColumnName("isAdmin")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");
            
            user.Property(u => u.MailUniqueHMac)
                .IsRequired()
                .HasColumnName("mailUniqueHMac")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);
            
            user.Property(u => u.PhoneNumberUniqueHMac)
                .IsRequired()
                .HasColumnName("phoneNumberUniqueHMac")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            user.Property(u => u.TriesBeforeLockedOut)
                .IsRequired()
                .HasColumnName("triesBeforeLockedOut")
                .HasColumnType("int");

            user.Property(u => u.IsActive)
                .IsRequired()
                .HasColumnName("isActive")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");

            user.Property(u => u.JTI)
                .IsRequired()
                .HasColumnName("jti")
                .HasColumnType("char(36)")
                .HasMaxLength(36);
            
            user.Property(u => u.SessionExpiresAt)
                .IsRequired()
                .HasColumnName("sessionExpiresAt")
                .HasColumnType("datetime");
            
            user.Property(u => u.SecurityQuestion)
                .IsRequired()
                .HasColumnName("securityQuestion")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);
            
            user.Property(u => u.SecurityAnswer)
                .IsRequired()
                .HasColumnName("securityAnswer")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);
            
            user.HasIndex(u => u.MailUniqueHMac).IsUnique();
            user.HasIndex(u => u.PhoneNumberUniqueHMac).IsUnique();
        });

        // ==================== CATEGORY ====================
        modelBuilder.Entity<Category>(category =>
        {
            category.ToTable("categories");
            category.HasKey(c => c.Id);

            category.Property(c => c.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            category.Property(c => c.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            category.Property(c => c.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            category.Property(c => c.ParentCategoryId)
                .HasColumnName("parentCategoryId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            category.Property(c => c.ImageUrl)
                .IsRequired()
                .HasColumnName("imageUrl")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            category.Property(c => c.IsActive)
                .IsRequired()
                .HasColumnName("isActive")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");

            category.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            // Relation auto-référentielle : 1 Category Parent -> N Categories Enfants
            category.HasOne<Category>()
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ==================== BRAND ====================
        modelBuilder.Entity<Brand>(brand =>
        {
            brand.ToTable("brands");
            brand.HasKey(b => b.Id);

            brand.Property(b => b.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            brand.Property(b => b.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            brand.Property(b => b.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            brand.Property(b => b.LogoUrl)
                .IsRequired()
                .HasColumnName("logoUrl")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            brand.Property(b => b.IsActive)
                .IsRequired()
                .HasColumnName("isActive")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");

            brand.Property(b => b.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");
        });

        // ==================== PRODUCT IMAGE ====================
        modelBuilder.Entity<ProductImage>(productImage =>
        {
            productImage.ToTable("product_images");
            productImage.HasKey(pi => pi.Id);

            productImage.Property(pi => pi.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            productImage.Property(pi => pi.ProductId)
                .IsRequired()
                .HasColumnName("productId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            productImage.Property(pi => pi.ImageUrl)
                .IsRequired()
                .HasColumnName("imageUrl")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            productImage.Property(pi => pi.DisplayOrder)
                .IsRequired()
                .HasColumnName("displayOrder")
                .HasColumnType("int");

            productImage.Property(pi => pi.IsPrimary)
                .IsRequired()
                .HasColumnName("isPrimary")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");

            productImage.Property(pi => pi.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            // Relation : 1 Product -> N ProductImages
            productImage.HasOne<Product>()
                .WithMany()
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== ORDER ====================
        modelBuilder.Entity<Order>(order =>
        {
            order.ToTable("orders");
            order.HasKey(o => o.Id);

            order.Property(o => o.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            order.Property(o => o.UserId)
                .IsRequired()
                .HasColumnName("userId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            order.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnName("totalAmount")
                .HasColumnType("decimal(10,2)");

            order.Property(o => o.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            order.Property(o => o.ShippingAddress)
                .IsRequired()
                .HasColumnName("shippingAddress")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            order.Property(o => o.ShippingZipCode)
                .IsRequired()
                .HasColumnName("shippingZipCode")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);

            order.Property(o => o.BillingAddress)
                .IsRequired()
                .HasColumnName("billingAddress")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            order.Property(o => o.BillingZipCode)
                .IsRequired()
                .HasColumnName("billingZipCode")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);

            order.Property(o => o.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            order.Property(o => o.ShippedAt)
                .HasColumnName("shippedAt")
                .HasColumnType("datetime");

            order.Property(o => o.DeliveredAt)
                .HasColumnName("deliveredAt")
                .HasColumnType("datetime");

            order.Property(o => o.TrackingNumber)
                .HasColumnName("trackingNumber")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            // Relation : 1 User -> N Orders (explicit pairing to avoid shadow FK like UserId1)
            order.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== ORDER ITEM ====================
        modelBuilder.Entity<OrderItem>(orderItem =>
        {
            orderItem.ToTable("order_items");
            orderItem.HasKey(oi => oi.Id);

            orderItem.Property(oi => oi.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            orderItem.Property(oi => oi.OrderId)
                .IsRequired()
                .HasColumnName("orderId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            orderItem.Property(oi => oi.ProductId)
                .IsRequired()
                .HasColumnName("productId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            orderItem.Property(oi => oi.Quantity)
                .IsRequired()
                .HasColumnName("quantity")
                .HasColumnType("int");

            orderItem.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnName("unitPrice")
                .HasColumnType("decimal(10,2)");

            orderItem.Property(oi => oi.TotalPrice)
                .IsRequired()
                .HasColumnName("totalPrice")
                .HasColumnType("decimal(10,2)");

            // Relations
            // 1 Order -> N OrderItems (explicit pairing to avoid shadow FK like OrderId1)
            orderItem.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 Product -> N OrderItems (explicitly bind navigation to avoid shadow FK like ProductId1)
            orderItem.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ==================== PAYMENT ====================
        modelBuilder.Entity<Payment>(payment =>
        {
            payment.ToTable("payments");
            payment.HasKey(p => p.Id);

            payment.Property(p => p.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            payment.Property(p => p.OrderId)
                .IsRequired()
                .HasColumnName("orderId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            payment.Property(p => p.Amount)
                .IsRequired()
                .HasColumnName("amount")
                .HasColumnType("decimal(10,2)");

            payment.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasColumnName("paymentMethod")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            payment.Property(p => p.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            payment.Property(p => p.TransactionId)
                .IsRequired()
                .HasColumnName("transactionId")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            payment.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            payment.Property(p => p.CompletedAt)
                .HasColumnName("completedAt")
                .HasColumnType("datetime");

            // Relation : 1 Order -> 1 Payment (1:1)
            // Make the relationship explicit to avoid EF one-to-one ambiguity
            payment.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== SHIPMENT ====================
        modelBuilder.Entity<Shipment>(shipment =>
        {
            shipment.ToTable("shipments");
            shipment.HasKey(s => s.Id);

            shipment.Property(s => s.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            shipment.Property(s => s.OrderId)
                .IsRequired()
                .HasColumnName("orderId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            shipment.Property(s => s.Carrier)
                .IsRequired()
                .HasColumnName("carrier")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            shipment.Property(s => s.TrackingNumber)
                .IsRequired()
                .HasColumnName("trackingNumber")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            shipment.Property(s => s.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            shipment.Property(s => s.ShippedAt)
                .IsRequired()
                .HasColumnName("shippedAt")
                .HasColumnType("datetime");

            shipment.Property(s => s.EstimatedDeliveryDate)
                .HasColumnName("estimatedDeliveryDate")
                .HasColumnType("datetime");

            shipment.Property(s => s.DeliveredAt)
                .HasColumnName("deliveredAt")
                .HasColumnType("datetime");

            // Relation : 1 Order -> 1 Shipment (1:1)
            shipment.HasOne<Order>()
                .WithOne()
                .HasForeignKey<Shipment>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== RETURN REQUEST ====================
        modelBuilder.Entity<ReturnRequest>(returnRequest =>
        {
            returnRequest.ToTable("return_requests");
            returnRequest.HasKey(rr => rr.Id);

            returnRequest.Property(rr => rr.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            returnRequest.Property(rr => rr.OrderId)
                .IsRequired()
                .HasColumnName("orderId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            returnRequest.Property(rr => rr.UserId)
                .IsRequired()
                .HasColumnName("userId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            returnRequest.Property(rr => rr.Reason)
                .IsRequired()
                .HasColumnName("reason")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            returnRequest.Property(rr => rr.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            returnRequest.Property(rr => rr.RequestedAt)
                .IsRequired()
                .HasColumnName("requestedAt")
                .HasColumnType("datetime");

            returnRequest.Property(rr => rr.ProcessedAt)
                .HasColumnName("processedAt")
                .HasColumnType("datetime");

            returnRequest.Property(rr => rr.AdminNotes)
                .HasColumnName("adminNotes")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            // Relations
            // 1 Order -> N ReturnRequests
            returnRequest.HasOne<Order>()
                .WithMany()
                .HasForeignKey(rr => rr.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 User -> N ReturnRequests
            returnRequest.HasOne<User>()
                .WithMany()
                .HasForeignKey(rr => rr.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ==================== CART ====================
        modelBuilder.Entity<Cart>(cart =>
        {
            cart.ToTable("carts");
            cart.HasKey(c => c.Id);

            cart.Property(c => c.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            cart.Property(c => c.UserId)
                .IsRequired()
                .HasColumnName("userId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            cart.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            cart.Property(c => c.UpdatedAt)
                .IsRequired()
                .HasColumnName("updatedAt")
                .HasColumnType("datetime");

            cart.HasIndex(c => c.UserId).IsUnique();

            // Relation : 1 User -> 1 Cart (1:1)
            cart.HasOne<User>()
                .WithOne()
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== CART ITEM ====================
        modelBuilder.Entity<CartItem>(cartItem =>
        {
            cartItem.ToTable("cart_items");
            cartItem.HasKey(ci => ci.Id);

            cartItem.Property(ci => ci.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            cartItem.Property(ci => ci.CartId)
                .IsRequired()
                .HasColumnName("cartId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            cartItem.Property(ci => ci.ProductId)
                .IsRequired()
                .HasColumnName("productId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            cartItem.Property(ci => ci.Quantity)
                .IsRequired()
                .HasColumnName("quantity")
                .HasColumnType("int");

            cartItem.Property(ci => ci.AddedAt)
                .IsRequired()
                .HasColumnName("addedAt")
                .HasColumnType("datetime");

            // Relations
            // 1 Cart -> N CartItems
            cartItem.HasOne<Cart>()
                .WithMany()
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 Product -> N CartItems
            cartItem.HasOne<Product>()
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== WISHLIST ====================
        modelBuilder.Entity<Wishlist>(wishlist =>
        {
            wishlist.ToTable("wishlists");
            wishlist.HasKey(w => w.Id);

            wishlist.Property(w => w.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            wishlist.Property(w => w.UserId)
                .IsRequired()
                .HasColumnName("userId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            wishlist.Property(w => w.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            wishlist.Property(w => w.UpdatedAt)
                .IsRequired()
                .HasColumnName("updatedAt")
                .HasColumnType("datetime");

            wishlist.HasIndex(w => w.UserId).IsUnique();

            // Relation : 1 User -> 1 Wishlist (1:1)
            wishlist.HasOne<User>()
                .WithOne()
                .HasForeignKey<Wishlist>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== WISHLIST ITEM ====================
        modelBuilder.Entity<WishlistItem>(wishlistItem =>
        {
            wishlistItem.ToTable("wishlist_items");
            wishlistItem.HasKey(wi => wi.Id);

            wishlistItem.Property(wi => wi.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            wishlistItem.Property(wi => wi.WishlistId)
                .IsRequired()
                .HasColumnName("wishlistId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            wishlistItem.Property(wi => wi.ProductId)
                .IsRequired()
                .HasColumnName("productId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            wishlistItem.Property(wi => wi.AddedAt)
                .IsRequired()
                .HasColumnName("addedAt")
                .HasColumnType("datetime");

            // Relations
            // 1 Wishlist -> N WishlistItems
            wishlistItem.HasOne<Wishlist>()
                .WithMany()
                .HasForeignKey(wi => wi.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 Product -> N WishlistItems
            wishlistItem.HasOne<Product>()
                .WithMany()
                .HasForeignKey(wi => wi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== ADDRESS ====================
        modelBuilder.Entity<Address>(address =>
        {
            address.ToTable("addresses");
            address.HasKey(a => a.Id);

            address.Property(a => a.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            address.Property(a => a.UserId)
                .IsRequired()
                .HasColumnName("userId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            address.Property(a => a.Label)
                .IsRequired()
                .HasColumnName("label")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            address.Property(a => a.FullAddress)
                .IsRequired()
                .HasColumnName("fullAddress")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            address.Property(a => a.ZipCode)
                .IsRequired()
                .HasColumnName("zipCode")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);

            address.Property(a => a.City)
                .IsRequired()
                .HasColumnName("city")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            address.Property(a => a.Country)
                .IsRequired()
                .HasColumnName("country")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            address.Property(a => a.IsDefault)
                .IsRequired()
                .HasColumnName("isDefault")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");

            address.Property(a => a.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            // Relation : 1 User -> N Addresses
            address.HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== NOTIFICATION ====================
        modelBuilder.Entity<Notification>(notification =>
        {
            notification.ToTable("notifications");
            notification.HasKey(n => n.Id);

            notification.Property(n => n.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            notification.Property(n => n.UserId)
                .IsRequired()
                .HasColumnName("userId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            notification.Property(n => n.Type)
                .IsRequired()
                .HasColumnName("type")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            notification.Property(n => n.Title)
                .IsRequired()
                .HasColumnName("title")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            notification.Property(n => n.Message)
                .IsRequired()
                .HasColumnName("message")
                .HasColumnType("varchar(500)")
                .HasMaxLength(500);

            notification.Property(n => n.IsRead)
                .IsRequired()
                .HasColumnName("isRead")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");

            notification.Property(n => n.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            notification.Property(n => n.ReadAt)
                .HasColumnName("readAt")
                .HasColumnType("datetime");

            // Relation : 1 User -> N Notifications
            notification.HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== SEARCH HISTORY ====================
        modelBuilder.Entity<SearchHistory>(searchHistory =>
        {
            searchHistory.ToTable("search_histories");
            searchHistory.HasKey(sh => sh.Id);

            searchHistory.Property(sh => sh.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            searchHistory.Property(sh => sh.UserId)
                .IsRequired()
                .HasColumnName("userId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            searchHistory.Property(sh => sh.SearchQuery)
                .IsRequired()
                .HasColumnName("searchQuery")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);

            searchHistory.Property(sh => sh.ResultCount)
                .IsRequired()
                .HasColumnName("resultCount")
                .HasColumnType("int");

            searchHistory.Property(sh => sh.SearchedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            // Relation : 1 User -> N SearchHistories
            searchHistory.HasOne<User>()
                .WithMany()
                .HasForeignKey(sh => sh.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== COUPON ====================
        modelBuilder.Entity<Coupon>(coupon =>
        {
            coupon.ToTable("coupons");
            coupon.HasKey(c => c.Id);

            coupon.Property(c => c.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            coupon.Property(c => c.Code)
                .IsRequired()
                .HasColumnName("code")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50);

            coupon.Property(c => c.DiscountType)
                .IsRequired()
                .HasColumnName("discountType")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);

            coupon.Property(c => c.DiscountValue)
                .IsRequired()
                .HasColumnName("discountValue")
                .HasColumnType("decimal(10,2)");

            coupon.Property(c => c.MinimumPurchase)
                .HasColumnName("minimumPurchase")
                .HasColumnType("decimal(10,2)");

            coupon.Property(c => c.UsageLimit)
                .HasColumnName("usageLimit")
                .HasColumnType("int");

            coupon.Property(c => c.UsageCount)
                .IsRequired()
                .HasColumnName("usageCount")
                .HasColumnType("int");

            coupon.Property(c => c.ValidFrom)
                .IsRequired()
                .HasColumnName("validFrom")
                .HasColumnType("datetime");

            coupon.Property(c => c.ValidUntil)
                .IsRequired()
                .HasColumnName("validUntil")
                .HasColumnType("datetime");

            coupon.Property(c => c.IsActive)
                .IsRequired()
                .HasColumnName("isActive")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");

            coupon.HasIndex(c => c.Code).IsUnique();
        });

        // ==================== DISCOUNT ====================
        modelBuilder.Entity<Discount>(discount =>
        {
            discount.ToTable("discounts");
            discount.HasKey(d => d.Id);

            discount.Property(d => d.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            discount.Property(d => d.ProductId)
                .HasColumnName("productId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            discount.Property(d => d.CategoryId)
                .HasColumnName("categoryId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);
            
            discount.Property(d => d.DiscountType)
                .IsRequired()
                .HasColumnName("discountType")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);

            discount.Property(d => d.ValidFrom)
                .IsRequired()
                .HasColumnName("validFrom")
                .HasColumnType("datetime");

            discount.Property(d => d.ValidUntil)
                .IsRequired()
                .HasColumnName("validUntil")
                .HasColumnType("datetime");

            discount.Property(d => d.IsActive)
                .IsRequired()
                .HasColumnName("isActive")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");

            // Relation : 1 Product -> N Discounts
            discount.HasOne<Product>()
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==================== REVIEW ====================
        modelBuilder.Entity<Review>(review =>
        {
            review.ToTable("reviews");
            review.HasKey(r => r.Id);

            review.Property(r => r.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            review.Property(r => r.ProductId)
                .IsRequired()
                .HasColumnName("productId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            review.Property(r => r.UserId)
                .IsRequired()
                .HasColumnName("userId")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            review.Property(r => r.Rating)
                .IsRequired()
                .HasColumnName("rating")
                .HasColumnType("int");

            review.Property(r => r.Comment)
                .IsRequired()
                .HasColumnName("comment")
                .HasColumnType("varchar(1000)")
                .HasMaxLength(1000);

            review.Property(r => r.IsVerifiedPurchase)
                .IsRequired()
                .HasColumnName("isVerifiedPurchase")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");

            review.Property(r => r.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            // Relations
            // 1 Product -> N Reviews
            review.HasOne<Product>()
                .WithMany()
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 User -> N Reviews
            review.HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ==================== NEWSLETTER ====================
        modelBuilder.Entity<Newsletter>(newsletter =>
        {
            newsletter.ToTable("newsletters");
            newsletter.HasKey(n => n.Id);

            newsletter.Property(n => n.Id)
                .IsRequired()
                .HasColumnName("id")
                .HasColumnType("char(36)")
                .HasMaxLength(36);

            newsletter.Property(n => n.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            newsletter.Property(n => n.IsSubscribed)
                .IsRequired()
                .HasColumnName("isSubscribed")
                .HasConversion(v => v ? 1 : 0, v => v != 0)
                .HasColumnType("bit");

            newsletter.Property(n => n.SubscribedAt)
                .IsRequired()
                .HasColumnName("subscribedAt")
                .HasColumnType("datetime");

            newsletter.Property(n => n.UnsubscribedAt)
                .HasColumnName("unsubscribedAt")
                .HasColumnType("datetime");

            newsletter.HasIndex(n => n.Email).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}