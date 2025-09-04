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

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mon API", Version = "v1" });
});

builder.Services.AddScoped<ConcreteMemoryCharActionFilter>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>))
    .AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>))
    .AddScoped<IUserService, UserService>();

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

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Services.InitializeDatabase();
app.MapControllers();
app.Run();
