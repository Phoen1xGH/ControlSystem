using ControlSystem.DAL.Interfaces;
using ControlSystem.DAL.Repositories;
using ControlSystem.Domain.Entities;
using ControlSystem.Services.Implementations;
using ControlSystem.Services.Interfaces;

namespace ControlSystem.MainApp.Helpers
{
    public static class InirializeHelper
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<UserAccount>, UserAccountRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountService, UserAccountService>();
        }
    }
}
