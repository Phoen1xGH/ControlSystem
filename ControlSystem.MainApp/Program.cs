using ControlSystem.MainApp.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog настраивается целиком из appsettings.json (секция "Serilog").
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

builder.Services.AddControllersWithViews();

builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsProduction())
{
    builder.Configuration.InitializeProductionSecrets();

    await builder.InitializeRedisConnectionAsync();
}

builder.InitializeDbConnection();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Account/Login");
    });

builder.Services.InitializeRepositories();
builder.Services.InitializeServices();
builder.Services.AddHealthCheck();

var app = builder.Build();

app.UseStructuredRequestLogging();

app.MapHealthCheck();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Метрики HTTP-запросов (rate, latency, коды ответов) с метками маршрута.
app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

// Эндпоинт /metrics для скрейпа (VMPodScrape ходит на него).
app.MapMetrics();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
