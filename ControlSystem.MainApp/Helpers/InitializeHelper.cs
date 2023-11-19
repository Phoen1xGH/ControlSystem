using ControlSystem.DAL.Interfaces;
using ControlSystem.DAL.Repositories;
using ControlSystem.Domain.Entities;
using ControlSystem.Services.Implementations;
using ControlSystem.Services.Interfaces;

namespace ControlSystem.MainApp.Helpers
{
    public static class InitializeHelper
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<UserAccount>, UserAccountRepository>();
            services.AddScoped<IRepository<Workspace>, WorkspaceRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IBPMNGenerateService, BPMNGenerateService>();
            services.AddScoped<IWorkspaceService, WorkspaceService>();
        }
    }
}
