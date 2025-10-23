using RentMaster.Core.Auth.Interface;
using RentMaster.Core.Auth.service;

namespace RentMaster.Core.Auth
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthModule(this IServiceCollection services)
        {
            // Đăng ký service chính
            services.AddScoped<IAuthService, AuthService>();

            // Nếu sau này có thêm helper khác
            // services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}