// ---------------------------------------------
// Point d'entrée de l'application ASP.NET Core.
// Configure les services (DI), la base de données, les middlewares
// (Swagger, CORS, HTTPS redirection, Routing, Authorization) et mappe les contrôleurs.
// Swagger est exposé sous /swagger pour permettre la démo devant jury.
// ---------------------------------------------
using alderaan_shop.Data;
using alderaan_shop.Helpers.Database;
using alderaan_shop.Helpers.DTOsActionFilter;
using alderaan_shop.Repositories;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services;
using alderaan_shop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using alderaan_shop.Helpers.Encryption;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuration JWT/JWE
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Add services to the container.
builder.Services.AddRazorPages();
//"Default": "Information",
//"Microsoft.AspNetCore": "Warning",
builder.Services.AddDbContext<ApplicationDatabaseContext>(options =>
{
    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(10, 11, 4));

    options.UseMySql(connectionString, serverVersion)
        .EnableSensitiveDataLogging(false)
        .LogTo(Console.WriteLine, LogLevel.Warning);
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ConcreteMemoryCharActionFilter>();

});

// CORS (Development): allow Swagger/UI or other local clients
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mon API", Version = "v1" });

    // Déclaration du schéma de sécurité Bearer pour Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Entrer le jeton JWT au format: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[] {}
        }
    });
});

builder.Services.AddScoped<ConcreteMemoryCharActionFilter>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>))
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IOrderRepository, OrderRepository>()
    .AddScoped<IAddressRepository, AddressRepository>()
    .AddScoped<IBrandRepository, BrandRepository>()
    .AddScoped<ICartRepository, CartRepository>()
    .AddScoped<ICartItemRepository, CartItemRepository>()
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<ICouponRepository, CouponRepository>()
    .AddScoped<IDiscountRepository, DiscountRepository>()
    .AddScoped<INewsletterRepository, NewsletterRepository>()
    .AddScoped<INotificationRepository, NotificationRepository>()
    .AddScoped<IOrderItemRepository, OrderItemRepository>()
    .AddScoped<IPaymentRepository, PaymentRepository>()
    .AddScoped<IProductImageRepository, ProductImageRepository>()
    .AddScoped<IReturnRequestRepository, ReturnRequestRepository>()
    .AddScoped<IReviewRepository, ReviewRepository>()
    .AddScoped<ISearchHistoryRepository, SearchHistoryRepository>()
    .AddScoped<IShipmentRepository, ShipmentRepository>()
    .AddScoped<IWishlistRepository, WishlistRepository>()
    .AddScoped<IWishlistItemRepository, WishlistItemRepository>();

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>))
    .AddScoped<IUserService, UserService>()
    .AddScoped<IProductService, ProductService>()
    .AddScoped<IOrderService, OrderService>()
    .AddScoped<IAddressService, AddressService>()
    .AddScoped<ICouponService, CouponService>()
    .AddScoped<IDiscountService, DiscountService>()
    .AddScoped<INewsletterService, NewsletterService>()
    .AddScoped<INotificationService, NotificationService>()
    .AddScoped<IReturnRequestService, ReturnRequestService>()
    .AddScoped<IReviewService, ReviewService>()
    .AddScoped<ISearchHistoryService, SearchHistoryService>()
    .AddScoped<IShipmentService, ShipmentService>()
    .AddScoped<IWishlistService, WishlistService>()
    .AddScoped<IWishlistItemService, WishlistItemService>();

// AuthN
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSection = builder.Configuration.GetSection("Jwt");
    string issuer = jwtSection["Issuer"] ?? "alderaan_shop";
    string audience = jwtSection["Audience"] ?? "alderaan_shop_clients";
    string signingKey = jwtSection["SigningKey"] ?? string.Empty;
    string encKey = jwtSection["EncryptionKey"] ?? string.Empty;

    options.RequireHttpsMetadata = false; // pour tests locaux via Swagger
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30),
        // Permettre la validation de JWE: clé de déchiffrement
        TokenDecryptionKey = string.IsNullOrWhiteSpace(encKey) ? null : new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encKey))
    };
});

// DI token service
builder.Services.AddScoped<ITokenService, TokenService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mon API v1");
        c.RoutePrefix = "swagger"; // Accès via /swagger
});


app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseRouting();

// Enable CORS (Development)
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Services.InitializeDatabase();
app.MapControllers();
app.Run();
