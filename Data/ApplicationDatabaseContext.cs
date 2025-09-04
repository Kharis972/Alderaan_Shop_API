using alderaan_shop.Models;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Data;

public class ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        });
        
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
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            user.Property(u => u.LastName)
                .IsRequired()
                .HasColumnName("lastName")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            user.Property(u => u.Address)
                .IsRequired()
                .HasColumnName("address")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256);
            
            user.Property(u => u.ZipCode)
                .IsRequired()
                .HasColumnName("zipCode")
                .HasColumnType("varchar(20)")
                .HasMaxLength(20);
            
            user.Property(u => u.Mail)
                .IsRequired()
                .HasColumnName("mail")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);
            
            user.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasColumnName("phoneNumber")
                .HasColumnType("Varchar(50)")
                .HasMaxLength(50);
            
            user.Property(u => u.EncryptedPassword)
                .IsRequired()
                .HasColumnName("encryptedPassword")
                .HasColumnType("Varchar(256)")
                .HasMaxLength(256);

            user.Property(u => u.IsAdmin)
                .IsRequired()
                .HasColumnName("isAdmin")
                .HasConversion(
                    v => v ? 1 : 0,
                    v => v != 0
                )
                .HasColumnType("bit");
            
            user.Property(u => u.MailUniqueHMac)
                .IsRequired()
                .HasColumnName("mailUniqueHMac")
                .HasColumnType("Varchar(256)")
                .HasMaxLength(256);
            
            user.Property(u => u.PhoneNumberUniqueHMac)
                .IsRequired()
                .HasColumnName("phoneNumberUniqueHMac")
                .HasColumnType("Varchar(256)")
                .HasMaxLength(256);

            user.Property(u => u.IsActive)
                .IsRequired()
                .HasColumnName("isActive")
                .HasConversion(
                    v => v ? 1 : 0,
                    v => v != 0
                )
                .HasColumnType("bit");

            user.Property(u => u.JTI)
                .IsRequired()
                .HasColumnName("jti")
                .HasColumnType("Varchar(256)")
                .HasMaxLength(256);
            
            user.Property(u => u.SessionExpiresAt)
                .IsRequired()
                .HasColumnName("sessionExpiresAt")
                .HasColumnType("datetime");
            
            user.Property(u => u.SecurityQuestion)
                .IsRequired()
                .HasColumnName("securityQuestion")
                .HasColumnType("Varchar(256)")
                .HasMaxLength(256);
            
            user.Property(u => u.SecurityAnswer)
                .IsRequired()
                .HasColumnName("securityAnswer")
                .HasColumnType("Varchar(256)")
                .HasMaxLength(256);
            
            user.HasIndex(u => u.MailUniqueHMac)
                .IsUnique();
            
            user.HasIndex(u => u.PhoneNumberUniqueHMac)
                .IsUnique();

        });
    }
}