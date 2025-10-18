using alderaan_shop.Data;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Helpers.Database;

public static class DatabaseInitializer
{
    public static void InitializeDatabase(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        ApplicationDatabaseContext context = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();
        // Apply pending migrations and keep schema up to date
        context.Database.Migrate();
    }
}