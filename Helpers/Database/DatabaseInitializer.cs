using alderaan_shop.Data;

namespace alderaan_shop.Helpers.Database;

public static class DatabaseInitializer
{
    public static void InitializeDatabase(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        ApplicationDatabaseContext context = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();
        context.Database.EnsureCreated();
    }
}