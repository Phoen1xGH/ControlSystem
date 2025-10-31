using ControlSystem.DAL;
using ControlSystem.Migrator.Initializers;
using Microsoft.EntityFrameworkCore;

DbContextOptions<ControlSystemContext> options = DbConnectionBuilder.BuildDbConnection();

Console.WriteLine("Starting migration...");

using var context = new ControlSystemContext(options);
await context.Database.MigrateAsync();

Console.WriteLine("Migration completed!");