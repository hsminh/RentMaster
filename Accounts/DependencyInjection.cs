using RentMaster.Accounts.Validator; 
using RentMaster.Accounts.Repositories;
using RentMaster.Accounts.Services;

namespace RentMaster.Accounts
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountModule(this IServiceCollection services)
        {
            services.AddScoped<ConsumerRepository>();
            services.AddScoped<ConsumerService>();
            services.AddScoped<ConsumerValidator>(); 
            return services;
        }
    }
}