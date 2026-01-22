namespace Daira.Api.Extensions
{
    public static class AddServices
    {
        public static IServiceCollection ApplyServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DairaDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("conn1"));
            });
            return services;
        }
    }
}
