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
            services.AddScoped<IRepository<Board>, BoardRepository>();
            services.AddScoped<IRepository<Ticket>, TicketRepository>();
            services.AddScoped<IRepository<Tag>, TagsRepository>();
            services.AddScoped<IRepository<Priority>, PriorityRepository>();
            services.AddScoped<IRepository<Comment>, CommentRepository>();
            services.AddScoped<IRepository<FileAttachment>, FileRepository>();
            services.AddScoped<IRepository<Link>, LinkRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IBPMNGenerateService, BPMNGenerateService>();
            services.AddScoped<IWorkspaceService, WorkspaceService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IPriorityService, PriorityService>();
            services.AddScoped<ILinkService, LinkService>();
            services.AddScoped<IFileService, FileService>();
        }
    }
}
