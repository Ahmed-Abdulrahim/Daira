using Daira.Infrastructure.Persistence.SeedDataClass;

namespace Daira.Api.Extensions
{
    public static class AppServices
    {
        public static async Task ApplyMigrationWithSeed(this WebApplication app)
        {
            using var scoped = app.Services.CreateScope();
            var services = scoped.ServiceProvider;
            var dbContext = services.GetRequiredService<DairaDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var env = services.GetRequiredService<IWebHostEnvironment>();

            try
            {
                await dbContext.Database.MigrateAsync();
                await dbContext.SeedDataAsync(env.ContentRootPath, null);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error has occurred during migration or seeding!");
            }
        }
    }
}
