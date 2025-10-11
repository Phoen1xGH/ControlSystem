using ControlSystem.DAL;
using ControlSystem.MainApp.Options;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StackExchange.Redis;

namespace ControlSystem.MainApp.Helpers
{
    public static class DbInitializer
    {
        public static void InitializeDbConnection(this WebApplicationBuilder builder)
        {
            var dbOptions = builder.Configuration
                .GetSection(ProjectDbOptions.Section)
                .Get<ProjectDbOptions>();

            if (dbOptions == null || !dbOptions.IsValid())
                throw new ArgumentNullException("Отсутствуют данные для подключения к БД");

            //if (dbOptions == null || !dbOptions.IsValid())
            //    return;

            DbOptions mainDbOptions = dbOptions.MAIN!;

            NpgsqlConnectionStringBuilder dbStringBuilder = new NpgsqlConnectionStringBuilder()
            {
                Host = mainDbOptions.HOST,
                Port = mainDbOptions.PORT,
                Database = mainDbOptions.DB_NAME,
                Username = mainDbOptions.USER,
                Password = mainDbOptions.PASSWORD,
                IncludeErrorDetail = mainDbOptions.ERRORS,
            };

            builder.Services.AddDbContext<ControlSystemContext>(options => options.UseNpgsql(dbStringBuilder.ConnectionString));
        }

        public static async Task MigrateDbAsync(this IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ControlSystemContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
