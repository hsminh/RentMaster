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
            
            services.AddScoped<LandLordRepository>();
            services.AddScoped<LandLordService>();
            // services.AddScoped<LandLordValidator>(); 
            
            services.AddScoped<AdminRepository>();
            services.AddScoped<AdminService>();
            services.AddScoped<AdminValidator>(); 
            
            return services;
        }
    }
}