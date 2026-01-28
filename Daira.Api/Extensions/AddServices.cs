using Daira.Application.Services;

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
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<DairaDbContext>().AddDefaultTokenProviders();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            // services.AddScoped<AuthUseCase>();
            return services;
        }
    }
}
